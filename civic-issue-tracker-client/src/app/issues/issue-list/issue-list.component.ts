import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IssueService } from '../issue.service';

@Component({
  selector: 'app-issue-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.css']
})
export class IssueListComponent implements OnInit {
  issues: any[] = [];

  constructor(private issueService: IssueService) {}

  ngOnInit(): void {
    this.issueService.getMyIssues().subscribe(data => {
      this.issues = data;
    });
  }
}