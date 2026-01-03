import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { IssueService } from '../issue.service';

@Component({
  selector: 'app-issue-create',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './issue-create.component.html'
})
export class IssueCreateComponent {

  model: any = {
    title: '',
    description: '',
    category: '',
    latitude: null,
    longitude: null
  };

  selectedFile: File | null = null;

  constructor(
    private issueService: IssueService,
    private router: Router
  ) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  submit(form: any) {
    if (form.invalid) {
      return;
    }

    const formData = new FormData();

    formData.append('title', this.model.title);
    formData.append('description', this.model.description);
    formData.append('category', this.model.category);
    formData.append('latitude', this.model.latitude);
    formData.append('longitude', this.model.longitude);

    if (this.selectedFile) {
      formData.append('image', this.selectedFile);
    }

    this.issueService.createIssue(formData).subscribe({
      next: () => {
        this.router.navigate(['/issues']);
      },
      error: () => {
        alert('Failed to submit issue. Please try again.');
      }
    });
  }

}