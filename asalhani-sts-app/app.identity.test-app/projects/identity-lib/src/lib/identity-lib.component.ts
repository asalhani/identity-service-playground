import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'lib-identity-lib',
  template: `
    <div class="loginbg">
      <main>
        <router-outlet></router-outlet>
      </main>
    </div>

  `,
  styles: [
  ]
})
export class IdentityLibComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
