import { Component, NgModule, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth';
import { CommonModule } from '@angular/common';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatIconModule} from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';


@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [FormsModule, CommonModule, MatInputModule, MatFormFieldModule, ReactiveFormsModule, MatProgressSpinnerModule, MatIconModule],
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';

  constructor(public auth: AuthService, private router: Router) {}
ngOnInit() {
    if (this.auth.isAuthenticated) {  
      this.router.navigate(['/profile']);
    }}
  
  onSubmit() {
    this.auth.login(this.username, this.password).subscribe({
      next: () => this.router.navigate(['/profile']),
      error: () => this.error = 'Неверные учетные данные'
    });
  }
}