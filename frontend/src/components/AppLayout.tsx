import React from 'react';
import { Link, useLocation } from 'react-router-dom';

const navItems = [
  { to: '/admin/users', label: 'Admin · Users' },
  { to: '/secretary/calendar', label: 'Secretary · Calendar' },
  { to: '/doctor/worklist', label: 'Doctor · Worklist' },
  { to: '/doctor/patients/p-1001/timeline', label: 'Patient Timeline' },
  { to: '/doctor/studies/st-1001', label: 'Study Detail' },
  { to: '/doctor/reports/rd-1001', label: 'Report Draft' }
];

export const AppLayout = ({ children }: { children: React.ReactNode }) => {
  const location = useLocation();

  return (
    <div style={{ fontFamily: 'Segoe UI, Arial, sans-serif', padding: 16 }}>
      <header style={{ marginBottom: 16 }}>
        <h1 style={{ margin: '0 0 8px 0' }}>Medical Gastro Clinic</h1>
        <nav style={{ display: 'flex', gap: 8, flexWrap: 'wrap' }}>
          {navItems.map((item) => {
            const active = location.pathname === item.to;
            return (
              <Link
                key={item.to}
                to={item.to}
                style={{
                  padding: '6px 10px',
                  borderRadius: 8,
                  border: '1px solid #cbd5e1',
                  textDecoration: 'none',
                  color: '#0f172a',
                  background: active ? '#dbeafe' : '#f8fafc'
                }}
              >
                {item.label}
              </Link>
            );
          })}
        </nav>
      </header>
      <section>{children}</section>
    </div>
  );
};
