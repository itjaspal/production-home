import { LoaderService } from './_service/loader.service';
import * as jQuery from 'jquery';
import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router, NavigationEnd } from '@angular/router';
import { APPCONFIG } from './config';
import { LayoutService } from './layout/layout.service';

// 3rd
import 'styles/material2-theme.scss';
import 'styles/bootstrap.scss';
// custom
import 'styles/layout.scss';
import 'styles/theme.scss';
import 'styles/ui.scss';
import 'styles/app.scss';

import * as moment from 'moment-timezone';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [LayoutService],
})
export class AppComponent implements OnInit {
  public AppConfig: any;
  public showLoader: boolean = false;

  constructor(
    private router: Router,
    private loaderSvc: LoaderService
  ) { }

  // jun = moment();

  ngOnInit() {
    this.AppConfig = APPCONFIG;

    // Scroll to top on route change
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });

    this.loaderSvc.status.subscribe((val: boolean) => {
      this.showLoader = val;
    });

    Date.prototype.toJSON = function () {
      return moment(this).tz('Asia/Bangkok').format();
    }
  }
}
