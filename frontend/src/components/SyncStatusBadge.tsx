import React from 'react';
export const SyncStatusBadge = ({ pendingCount }: { pendingCount: number }) => (
  <span>Pending sync: {pendingCount}</span>
);
