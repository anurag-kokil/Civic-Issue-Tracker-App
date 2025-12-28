import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IssueService, Issue } from '../../issues/issue.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.component.html'
})
export class AdminDashboardComponent implements OnInit {

  issues: Issue[] = [];
  loading = true;

  // statuses = ['Reported','Assigned', 'InProgress', 'Resolved', 'Closed'];

  allowedTransitions: Record<string, string[]> = {
    Reported: ['Assigned'],
    Assigned: ['InProgress'],
    InProgress: ['Resolved'],
    Resolved: ['Closed'],
    Closed: []
  };


  constructor(private issueService: IssueService) {}

  ngOnInit(): void {
    this.loadIssues();
  }

  loadIssues() {
    this.issueService.getAllIssues().subscribe(data => {
      this.issues = data;
      this.loading = false;
    });
  }

  // updateStatus(issue: Issue, newStatus: string) {
  //   this.issueService.updateStatus(issue.id, newStatus).subscribe(() => {
  //     issue.status = newStatus; // UI update
  //   });
  // }

  updateStatus(issue: Issue, newStatus: string) {
  if (!newStatus || newStatus === issue.status) return;

  this.issueService.updateStatus(issue.id, newStatus).subscribe({
    next: () => {
      // ✅ Update UI immediately
      issue.status = newStatus;

      // 🔁 Optional but robust: reload list
      this.loadIssues();
    },
    error: () => {
      alert('Failed to update status');
    }
  });
}

  getNextStatuses(currentStatus: string): string[] {
    return this.allowedTransitions[currentStatus] || [];
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Reported': return 'bg-secondary';
      case 'Assigned': return 'bg-info';
      case 'InProgress': return 'bg-warning';
      case 'Resolved': return 'bg-success';
      case 'Closed': return 'bg-dark';
      default: return 'bg-secondary';
    }
}


}
