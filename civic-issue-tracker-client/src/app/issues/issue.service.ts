import { HttpClient } from '@angular/common/http';
import {  Injectable } from '@angular/core';

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

}