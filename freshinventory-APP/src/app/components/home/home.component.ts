import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule,
    BsDropdownModule
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  features = [
    {
      icon: 'chart-line',
      title: 'Real-time Analytics',
      description: 'Track your inventory metrics with powerful analytics and insights.'
    },
    {
      icon: 'box',
      title: 'Smart Stock Management',
      description: 'Efficiently manage your ingredients with automated tracking.'
    },
    {
      icon: 'clock',
      title: 'Expiry Tracking',
      description: 'Never waste ingredients with our smart expiry date monitoring.'
    },
    {
      icon: 'chart-pie',
      title: 'Cost Analysis',
      description: 'Optimize your costs with detailed financial reporting.'
    }
  ];
}