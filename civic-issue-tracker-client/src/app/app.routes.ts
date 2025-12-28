import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { IssueListComponent } from './issues/issue-list/issue-list.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'issues', component: IssueListComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
