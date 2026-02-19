import React from 'react';
import { TimelineFeed } from '../components/TimelineFeed';

const timeline = [
  { id: 't1', at: '2026-02-11 09:20', label: 'Appointment marked Ready' },
  { id: 't2', at: '2026-02-11 09:30', label: 'Blood test PDF attached to Study st-2002' },
  { id: 't3', at: '2026-02-11 09:36', label: 'AI summary generated with confidence 0.87' }
];

export const PatientTimelinePage = () => (
  <main>
    <h2>Patient Timeline</h2>
    <p>Unified timeline across appointments, studies, attachments, and AI insights.</p>
    <TimelineFeed items={timeline} />
  </main>
);
