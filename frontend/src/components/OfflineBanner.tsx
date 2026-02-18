import React from 'react';
export const OfflineBanner = ({ offline }: { offline: boolean }) => {
  if (!offline) return null;
  return <div role="status">Offline mode enabled. Changes will sync automatically.</div>;
};
