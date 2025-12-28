import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { IssueListComponent } from './issues/issue-list/issue-list.component';
import { IssueCreateComponent } from './issues/issue-create/issue-create.component';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'issues', component: IssueListComponent, canActivate: [authGuard] },
  { path: 'issues/create', component: IssueCreateComponent, canActivate: [authGuard] },
  { path: 'admin', component: AdminDashboardComponent, canActivate: [authGuard, roleGuard(['Admin', 'Officer'])] },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
