import React from 'react';
import { AttachmentUploader } from '../components/AttachmentUploader';
import { AiInsightPanel } from '../components/AiInsightPanel';

export const StudyDetailPage = () => (
  <main>
    <h2>Study Detail</h2>
    <p>Study Type: Labs · Status: Draft · Ordered By: Dr. Samira Rashed</p>
    <AttachmentUploader />
    <AiInsightPanel />
  </main>
);
