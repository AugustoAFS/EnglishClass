import { Component, Inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  imports: [RouterLink] // Adicione os módulos necessários
})
export class HeaderComponent implements OnInit {
  login: boolean = false;

  constructor(private router: Router, @Inject(PLATFORM_ID) private platformId: Object) {}

  ngOnInit() {
    this.verificarLogin();
  }

  verificarLogin() {
    if (isPlatformBrowser(this.platformId)) {
      // Verifique se estamos no lado do cliente (navegador)
      this.login = localStorage.getItem('jwtToken') !== null;
    }
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      // Verifique se estamos no lado do cliente (navegador)
      localStorage.removeItem('jwtToken');
      this.login = false;
      this.router.navigate(['/login']);
    }
  }
}
