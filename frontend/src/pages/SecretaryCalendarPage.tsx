import React from 'react';
import { OfflineBanner } from '../components/OfflineBanner';
import { SyncStatusBadge } from '../components/SyncStatusBadge';

const slots = [
  { time: '10:00', provider: 'Dr. Samira Rashed', state: 'Available' },
  { time: '10:20', provider: 'Dr. Samira Rashed', state: 'Booked' },
  { time: '10:40', provider: 'Dr. Omar Nassar', state: 'Available' }
];

export const SecretaryCalendarPage = () => (
  <main>
    <OfflineBanner offline={false} />
    <h2>Secretary Calendar</h2>
    <SyncStatusBadge pendingCount={0} />
    <p>Book, reschedule, or cancel appointments from free slots.</p>

    <ul>
      {slots.map((slot) => (
        <li key={`${slot.provider}-${slot.time}`}>
          {slot.time} — {slot.provider} — <strong>{slot.state}</strong>
        </li>
      ))}
    </ul>

    <button type="button">Create Appointment</button>
  </main>
);
