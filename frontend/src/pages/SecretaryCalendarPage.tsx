import { OfflineBanner } from '../components/OfflineBanner';
import { SyncStatusBadge } from '../components/SyncStatusBadge';

export const SecretaryCalendarPage = () => (
  <main>
    <OfflineBanner offline={false} />
    <h1>Secretary Calendar</h1>
    <SyncStatusBadge pendingCount={0} />
  </main>
);
