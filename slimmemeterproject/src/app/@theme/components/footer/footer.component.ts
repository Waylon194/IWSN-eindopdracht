import { Component } from '@angular/core';

@Component({
  selector: 'ngx-footer',
  styleUrls: ['./footer.component.scss'],
  template: `
    <span class="created-by">
      Built using ngx-admin by <b><a href="https://akveo.page.link/8V2f" target="_blank">Akveo</a></b> 2021
    </span>
  `,
})
export class FooterComponent {
}
