import React from 'react';
export const TimelineFeed = ({ items }: { items: Array<{ id: string; label: string; at: string }> }) => (
  <ul>
    {items.map((i) => (
      <li key={i.id}>{i.at} - {i.label}</li>
    ))}
  </ul>
);
