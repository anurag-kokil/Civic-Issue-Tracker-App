import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);

    const role = auth.getUserRole();

    if (role && allowedRoles.includes(role)) {
      return true;
    }

    return router.parseUrl('/issues');
  };
};