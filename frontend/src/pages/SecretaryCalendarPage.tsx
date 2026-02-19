import React, { useMemo, useState } from 'react';
import { OfflineBanner } from '../components/OfflineBanner';
import { SyncStatusBadge } from '../components/SyncStatusBadge';
import { apiClient } from '../api/client';

type Slot = {
  time: string;
  providerId: string;
  providerName: string;
  state: 'Available' | 'Booked';
};

const slots: Slot[] = [
  { time: '10:00', providerId: 'doctor-1', providerName: 'Dr. Samira Rashed', state: 'Available' },
  { time: '10:20', providerId: 'doctor-1', providerName: 'Dr. Samira Rashed', state: 'Booked' },
  { time: '10:40', providerId: 'doctor-2', providerName: 'Dr. Omar Nassar', state: 'Available' }
];

export const SecretaryCalendarPage = () => {
  const [medicalCenterId, setMedicalCenterId] = useState('center-a');
  const [patientId, setPatientId] = useState('patient-1001');
  const [secretaryId, setSecretaryId] = useState('secretary-1');
  const [selectedSlot, setSelectedSlot] = useState<Slot | null>(slots.find((x) => x.state === 'Available') ?? null);
  const [result, setResult] = useState<string>('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const availableSlots = useMemo(() => slots.filter((x) => x.state === 'Available'), []);

  const createAppointment = async () => {
    if (!selectedSlot) return;

    setIsSubmitting(true);
    setResult('');

    const today = new Date();
    const [hour, minute] = selectedSlot.time.split(':').map(Number);
    const start = new Date(today.getFullYear(), today.getMonth(), today.getDate(), hour, minute, 0);
    const end = new Date(start.getTime() + 20 * 60 * 1000);

    const payload = {
      appointmentId: crypto.randomUUID(),
      medicalCenterId,
      patientId,
      providerId: selectedSlot.providerId,
      secretaryId,
      startUtc: start.toISOString(),
      endUtc: end.toISOString(),
      notes: 'Booked from secretary calendar UI',
      idempotencyKey: crypto.randomUUID()
    };

    try {
      const response = await apiClient.post('/appointments', payload);
      const sync = (response.data.calendarSync ?? []) as Array<{ userId: string; calendarId: string; eventId: string }>;
      const syncSummary = sync.map((x) => `${x.userId} -> ${x.calendarId} (${x.eventId})`).join(' | ');
      setResult(`Appointment created and synced to Google calendars: ${syncSummary}`);
    } catch (error: unknown) {
      const message = error instanceof Error ? error.message : 'Unable to create appointment';
      setResult(`Create appointment failed: ${message}`);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <main>
      <OfflineBanner offline={false} />
      <h2>Secretary Calendar</h2>
      <SyncStatusBadge pendingCount={0} />
      <p>Book, reschedule, or cancel appointments from free slots with Google Calendar sync.</p>

      <div style={{ display: 'grid', gap: 8, maxWidth: 680, marginBottom: 12 }}>
        <label>
          Medical center:
          <select value={medicalCenterId} onChange={(e) => setMedicalCenterId(e.target.value)}>
            <option value="center-a">center-a</option>
            <option value="center-b">center-b</option>
          </select>
        </label>

        <label>
          Secretary ID:
          <input value={secretaryId} onChange={(e) => setSecretaryId(e.target.value)} />
        </label>

        <label>
          Patient ID:
          <input value={patientId} onChange={(e) => setPatientId(e.target.value)} />
        </label>

        <label>
          Free slot:
          <select
            value={selectedSlot ? `${selectedSlot.providerId}|${selectedSlot.time}` : ''}
            onChange={(e) => {
              const [providerId, time] = e.target.value.split('|');
              setSelectedSlot(availableSlots.find((x) => x.providerId === providerId && x.time === time) ?? null);
            }}
          >
            {availableSlots.map((slot) => (
              <option key={`${slot.providerId}|${slot.time}`} value={`${slot.providerId}|${slot.time}`}>
                {slot.time} — {slot.providerName}
              </option>
            ))}
          </select>
        </label>
      </div>

      <ul>
        {slots.map((slot) => (
          <li key={`${slot.providerName}-${slot.time}`}>
            {slot.time} — {slot.providerName} — <strong>{slot.state}</strong>
          </li>
        ))}
      </ul>

      <button type="button" onClick={createAppointment} disabled={!selectedSlot || isSubmitting}>
        {isSubmitting ? 'Creating...' : 'Create Appointment'}
      </button>

      {result && <p role="status">{result}</p>}
    </main>
  );
};
