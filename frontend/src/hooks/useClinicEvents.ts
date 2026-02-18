import { useEffect } from 'react';

export const useClinicEvents = () => {
  useEffect(() => {
    // TODO: connect @microsoft/signalr HubConnection and subscribe to queue, AI, report events.
    return () => {
      // cleanup
    };
  }, []);
};
