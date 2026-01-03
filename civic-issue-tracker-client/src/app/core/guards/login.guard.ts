import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

export const loginGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  // If already logged in → redirect
  if (auth.isLoggedIn()) {
    return router.parseUrl('/issues');
  }

  // Otherwise allow access to login
  return true;
};
