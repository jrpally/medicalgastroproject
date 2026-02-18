import axios from 'axios';

export const apiClient = axios.create({
  baseURL: '/api',
  timeout: 15000
});

export const withIdempotency = (idempotencyKey: string) => ({
  headers: { 'x-idempotency-key': idempotencyKey }
});
