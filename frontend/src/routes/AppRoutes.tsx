import React from 'react';
import { Navigate, Route, Routes } from 'react-router-dom';
import { SecretaryCalendarPage } from '../pages/SecretaryCalendarPage';
import { DoctorWorklistPage } from '../pages/DoctorWorklistPage';
import { PatientTimelinePage } from '../pages/PatientTimelinePage';
import { StudyDetailPage } from '../pages/StudyDetailPage';
import { ReportDraftPage } from '../pages/ReportDraftPage';

export const AppRoutes = () => (
  <Routes>
    <Route path="/" element={<Navigate to="/doctor/worklist" replace />} />
    <Route path="/secretary/calendar" element={<SecretaryCalendarPage />} />
    <Route path="/doctor/worklist" element={<DoctorWorklistPage />} />
    <Route path="/doctor/patients/:id/timeline" element={<PatientTimelinePage />} />
    <Route path="/doctor/studies/:studyId" element={<StudyDetailPage />} />
    <Route path="/doctor/reports/:draftId" element={<ReportDraftPage />} />
    <Route path="*" element={<Navigate to="/doctor/worklist" replace />} />
  </Routes>
);
