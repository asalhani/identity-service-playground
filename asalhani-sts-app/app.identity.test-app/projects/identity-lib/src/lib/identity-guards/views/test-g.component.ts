import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-g',
  template: `
    <p>
      this is test component from guard module
    </p>
  `,
  styles: [
  ]
})
export class TestGComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
