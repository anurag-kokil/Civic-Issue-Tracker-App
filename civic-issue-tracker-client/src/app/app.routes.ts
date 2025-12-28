import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { IssueListComponent } from './issues/issue-list/issue-list.component';
import { IssueCreateComponent } from './issues/issue-create/issue-create.component';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'issues', component: IssueListComponent },
  { path: 'issues/create', component: IssueCreateComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
