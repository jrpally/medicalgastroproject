import React from 'react';
export type ConflictItem = { id: string; entityType: string; reason: string };

export const ConflictResolutionDrawer = ({ items }: { items: ConflictItem[] }) => (
  <aside>
    <h3>Conflicts</h3>
    <ul>
      {items.map((i) => (
        <li key={i.id}>{i.entityType}: {i.reason}</li>
      ))}
    </ul>
  </aside>
);
