import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IssueService } from '../issue.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-issue-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.css']
})
export class IssueListComponent implements OnInit {
  issues: any[] = [];
  loading: boolean = false;
  error: string | null = null;

  constructor(private issueService: IssueService) {}

  ngOnInit(): void {
    this.issueService.getMyIssues().subscribe(data => {
      this.issues = data;
    });
  }
}