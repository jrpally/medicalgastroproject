import React, { useMemo, useState } from 'react';

type StaffRole = 'Doctor' | 'Secretary';

type StaffMember = {
  id: string;
  fullName: string;
  role: StaffRole;
  specialty?: string;
  active: boolean;
};

const initialStaff: StaffMember[] = [
  { id: 'u-1001', fullName: 'Dr. Samira Rashed', role: 'Doctor', specialty: 'Gastroenterology', active: true },
  { id: 'u-1002', fullName: 'Dr. Omar Nassar', role: 'Doctor', specialty: 'Internal Medicine', active: true },
  { id: 'u-2001', fullName: 'Nora Khaled', role: 'Secretary', active: true }
];

export const AdminUsersPage = () => {
  const [staff, setStaff] = useState<StaffMember[]>(initialStaff);
  const [form, setForm] = useState({ fullName: '', role: 'Doctor' as StaffRole, specialty: '' });

  const doctors = useMemo(() => staff.filter((x) => x.role === 'Doctor'), [staff]);
  const secretaries = useMemo(() => staff.filter((x) => x.role === 'Secretary'), [staff]);

  const onAdd = () => {
    if (!form.fullName.trim()) return;

    const item: StaffMember = {
      id: `u-${Date.now()}`,
      fullName: form.fullName.trim(),
      role: form.role,
      specialty: form.role === 'Doctor' ? form.specialty.trim() || 'General' : undefined,
      active: true
    };

    setStaff((prev) => [item, ...prev]);
    setForm({ fullName: '', role: 'Doctor', specialty: '' });
  };

  const toggleActive = (id: string) => {
    setStaff((prev) => prev.map((x) => (x.id === id ? { ...x, active: !x.active } : x)));
  };

  return (
    <main>
      <h2>Admin · Staff Management</h2>
      <p>Add doctors, secretaries, and manage active/inactive status.</p>

      <section style={{ border: '1px solid #cbd5e1', borderRadius: 8, padding: 12, marginBottom: 16 }}>
        <h3 style={{ marginTop: 0 }}>Create staff account</h3>
        <div style={{ display: 'grid', gap: 8, maxWidth: 520 }}>
          <input
            placeholder="Full name"
            value={form.fullName}
            onChange={(e) => setForm((p) => ({ ...p, fullName: e.target.value }))}
          />
          <select
            value={form.role}
            onChange={(e) => setForm((p) => ({ ...p, role: e.target.value as StaffRole }))}
          >
            <option value="Doctor">Doctor</option>
            <option value="Secretary">Secretary</option>
          </select>
          {form.role === 'Doctor' && (
            <input
              placeholder="Specialty"
              value={form.specialty}
              onChange={(e) => setForm((p) => ({ ...p, specialty: e.target.value }))}
            />
          )}
          <button type="button" onClick={onAdd}>Add staff member</button>
        </div>
      </section>

      <section style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
        <div style={{ border: '1px solid #cbd5e1', borderRadius: 8, padding: 12 }}>
          <h3 style={{ marginTop: 0 }}>Doctors ({doctors.length})</h3>
          <ul>
            {doctors.map((d) => (
              <li key={d.id}>
                <strong>{d.fullName}</strong> — {d.specialty} — {d.active ? 'Active' : 'Inactive'}{' '}
                <button type="button" onClick={() => toggleActive(d.id)}>
                  {d.active ? 'Disable' : 'Enable'}
                </button>
              </li>
            ))}
          </ul>
        </div>

        <div style={{ border: '1px solid #cbd5e1', borderRadius: 8, padding: 12 }}>
          <h3 style={{ marginTop: 0 }}>Secretaries ({secretaries.length})</h3>
          <ul>
            {secretaries.map((s) => (
              <li key={s.id}>
                <strong>{s.fullName}</strong> — {s.active ? 'Active' : 'Inactive'}{' '}
                <button type="button" onClick={() => toggleActive(s.id)}>
                  {s.active ? 'Disable' : 'Enable'}
                </button>
              </li>
            ))}
          </ul>
        </div>
      </section>
    </main>
  );
};
