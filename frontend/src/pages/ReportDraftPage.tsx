import React from 'react';

export const ReportDraftPage = () => (
  <main>
    <h2>Report Draft</h2>
    <p>AI-generated draft report from selected study attachments. Review before finalizing.</p>
    <textarea
      defaultValue={'Impression:\n- Mild gastritis changes noted.\n- Correlate with lab values and symptoms.'}
      rows={8}
      style={{ width: '100%', maxWidth: 900 }}
    />
    <div style={{ marginTop: 8 }}>
      <button type="button">Save Draft</button>{' '}
      <button type="button">Finalize Report</button>
    </div>
  </main>
);
