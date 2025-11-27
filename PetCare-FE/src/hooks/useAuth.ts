import { useAuth0 } from '@auth0/auth0-react';
import type { Auth0Context } from '../types/auth0';

export const useAuth = (): Auth0Context => {
  const context = useAuth0();
  
  if (!context) {
    throw new Error('useAuth must be used within an Auth0Provider');
  }
  
  return context;
};