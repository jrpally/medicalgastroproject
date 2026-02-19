import React from 'react';
import { Navigate, Route, Routes } from 'react-router-dom';
import { AppLayout } from '../components/AppLayout';
import { AdminUsersPage } from '../pages/AdminUsersPage';
import { SecretaryCalendarPage } from '../pages/SecretaryCalendarPage';
import { DoctorWorklistPage } from '../pages/DoctorWorklistPage';
import { PatientTimelinePage } from '../pages/PatientTimelinePage';
import { StudyDetailPage } from '../pages/StudyDetailPage';
import { ReportDraftPage } from '../pages/ReportDraftPage';

export const AppRoutes = () => (
  <AppLayout>
    <Routes>
      <Route path="/" element={<Navigate to="/admin/users" replace />} />
      <Route path="/admin/users" element={<AdminUsersPage />} />
      <Route path="/secretary/calendar" element={<SecretaryCalendarPage />} />
      <Route path="/doctor/worklist" element={<DoctorWorklistPage />} />
      <Route path="/doctor/patients/:id/timeline" element={<PatientTimelinePage />} />
      <Route path="/doctor/studies/:studyId" element={<StudyDetailPage />} />
      <Route path="/doctor/reports/:draftId" element={<ReportDraftPage />} />
      <Route path="*" element={<Navigate to="/admin/users" replace />} />
    </Routes>
  </AppLayout>
);
