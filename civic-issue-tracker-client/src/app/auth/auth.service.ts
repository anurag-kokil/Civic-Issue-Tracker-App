import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5088/api/auth';
  private isBrowser: boolean;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  login(data: any) {
    return this.http.post<any>(`${this.apiUrl}/login`, data);
  }

  saveToken(token: string) {
    if (this.isBrowser) {
      localStorage.setItem('token', token);
    }
  }

  getToken(): string | null {
    if (this.isBrowser) {
      return localStorage.getItem('token');
    }
    return null;
  }

  logout() {
    if (this.isBrowser) {
      localStorage.removeItem('token');
    }
  }

  isLoggedIn(): boolean {
    return this.isBrowser && !!this.getToken();
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));

    // ASP.NET Core role claim
    return (
      payload['role'] ||
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
      null
    );
  }
}