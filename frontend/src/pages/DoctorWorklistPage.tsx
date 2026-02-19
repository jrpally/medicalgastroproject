import React from 'react';

const queueItems = [
  { id: 'ap-1001', patient: 'Mona Adel', slot: '09:00', status: 'Arrived', studyType: 'Lab Review' },
  { id: 'ap-1002', patient: 'Hassan Ibrahim', slot: '09:20', status: 'Ready', studyType: 'Endoscopy Follow-up' },
  { id: 'ap-1003', patient: 'Rania Mostafa', slot: '09:40', status: 'In Progress', studyType: 'Consultation Note' }
];

export const DoctorWorklistPage = () => (
  <main>
    <h2>Doctor Worklist</h2>
    <p>Live queue status (SignalR-ready) with current appointments and study context.</p>

    <table style={{ borderCollapse: 'collapse', width: '100%', maxWidth: 900 }}>
      <thead>
        <tr>
          <th style={{ borderBottom: '1px solid #cbd5e1', textAlign: 'left', padding: 8 }}>Time</th>
          <th style={{ borderBottom: '1px solid #cbd5e1', textAlign: 'left', padding: 8 }}>Patient</th>
          <th style={{ borderBottom: '1px solid #cbd5e1', textAlign: 'left', padding: 8 }}>Study Type</th>
          <th style={{ borderBottom: '1px solid #cbd5e1', textAlign: 'left', padding: 8 }}>Status</th>
        </tr>
      </thead>
      <tbody>
        {queueItems.map((item) => (
          <tr key={item.id}>
            <td style={{ borderBottom: '1px solid #e2e8f0', padding: 8 }}>{item.slot}</td>
            <td style={{ borderBottom: '1px solid #e2e8f0', padding: 8 }}>{item.patient}</td>
            <td style={{ borderBottom: '1px solid #e2e8f0', padding: 8 }}>{item.studyType}</td>
            <td style={{ borderBottom: '1px solid #e2e8f0', padding: 8 }}>{item.status}</td>
          </tr>
        ))}
      </tbody>
    </table>
  </main>
);
