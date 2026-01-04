import { HttpClient } from '@angular/common/http';
import {  Injectable } from '@angular/core';

export interface Issue {
  id: number;
  title: string;
  description?: string;
  category: string;
  status: string;
  createdAt: string;
  reportedByUserId?: number;
  imageUrl?: string;
  latitude?: number;
  longitude?: number;
  assignedOfficerId?: number;
  assignedOfficer?: {
    id: number;
    name: string;
    email: string;
  };
}

@Injectable({ providedIn: 'root' })
export class IssueService {
  private apiUrl = 'http://localhost:5088/api/issues';

  constructor(private http: HttpClient) {}

  getMyIssues() {
    return this.http.get<any[]>(`${this.apiUrl}/my`);
  }

  createIssue(data: FormData) {
    return this.http.post(
      'http://localhost:5088/api/issues',
      data
    );
  }

  /** Admin / Officer: get all issues */
  getAllIssues() {
    return this.http.get<Issue[]>(
      'http://localhost:5088/api/issues'
    );
  }

  /** Admin update issue status */
  updateStatus(issueId: number, newStatus: string) {
    return this.http.put(
      `http://localhost:5088/api/issues/${issueId}/status`,
      { newStatus },
      { responseType: 'text' }
    );
  }

  getOfficers() {
    return this.http.get<any[]>(
      'http://localhost:5088/api/users/officers'
    );
  }

  assignIssue(issueId: number, officerId: number) {
    return this.http.put(
      `http://localhost:5088/api/issues/${issueId}/assign/${officerId}`,
      {},
      { responseType: 'text' }
    );
  }

}