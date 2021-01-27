import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { JobOperationSearchView, JobOperationDetailView, JobOperationDataTotalView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { JobOperationService } from '../../_service/job-operation.service';

@Component({
  selector: 'app-inprocess',
  templateUrl: './inprocess.component.html',
  styleUrls: ['./inprocess.component.scss']
})
export class InprocessComponent implements OnInit {

  @ViewChild('req_date') req_date: ElementRef;
  
  constructor(
    private _jobOperationMacSvc: JobOperationService,
    private _authSvc: AuthenticationService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _dll: DropdownlistService,
  ) { }

  public model: JobOperationSearchView = new JobOperationSearchView();
  public dataCurrent: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  // public dataPending: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  // public dataForward: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  
  public user: any; 
  public datePipe = new DatePipe('en-US'); 
  public validationForm: FormGroup;
  public wclist: any; 

  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser(); 
    this.model.build_type = this.user.branch.entity_code;
    this.wclist = await this._dll.getDdlWCInprocess(this.user.username);
    
    this.searchJobOperation();
    
  }   

  buildForm() {
    this.validationForm = this._formBuilder.group({
      req_date: ['', Validators.compose([Validators.required, YourValidator.dateVaidator])],
      wc_code: [''],
      build_type: ['']
    });
  }

  close() {
    this._router.navigateByUrl('/app/mobile-navigator');  
  }
  
  ngOnDestroy() {
    //console.log("Close Program ");
    //sessionStorage.removeItem('spect-drawing-reqDate');
    //sessionStorage.removeItem('spect-drawing-pcsBarcode');
  }

  async searchJobOperation(event: PageEvent = null) {   

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }
    var datePipe = new DatePipe("en-US");
    this.model.user_id = this.user.username;
    this.dataCurrent.dataTotals  = [];
    // this.dataPending.dataTotals  = [];
    // this.dataForward.dataTotals  = [];

    sessionStorage.setItem('jobOperation-reqDate', "");
    sessionStorage.setItem('jobOperation-wcCode', "");
    sessionStorage.setItem('jobOperation-build_type', this.user.branch.entity_code);

    this.model.build_type = this.user.branch.entity_code;
    // this.model.wc_code    = "";
    this.model.req_date = "";
    

    if(this.wclist.length >= 1)
    {
      this.model.wc_code = this.wclist[0].key;
      
    }
   

    this.dataCurrent =  await this._jobOperationMacSvc.searchJobOperationCurrent(this.model);
    console.log(this.dataCurrent);
    // this.dataPending =  await this._jobOperationMacSvc.searchJobOperationPending(this.model);
    // console.log(this.dataPending);
    // this.dataForward =  await this._jobOperationMacSvc.searchJobOperationForward(this.model);
    // console.log(this.dataForward);
     
  }
  
  async searchJobOperationByParam(event: PageEvent = null) {   

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }
    
    /*console.log("current username : " + this.user.username);
    console.log("current build_type : " + this.user.branch.entity_code);*/
    
    this.model.user_id = this.user.username;
    this.dataCurrent.dataTotals  = [];
    // this.dataPending.dataTotals  = [];
    // this.dataForward.dataTotals  = [];

    sessionStorage.setItem('jobOperation-reqDate', this.req_date.nativeElement.value);
    sessionStorage.setItem('jobOperation-wcCode', this.validationForm.get('wc_code').value);
    sessionStorage.setItem('jobOperation-build_type', this.validationForm.get('build_type').value);
 
    this.model.build_type = this.validationForm.get('build_type').value;
    if (this.validationForm.get('wc_code').value == "0") {
        this.model.wc_code    = "";//"PH4";
    } else {
        this.model.wc_code    = this.validationForm.get('wc_code').value;//"PH4";
    }    
    this.model.req_date   = this.req_date.nativeElement.value;//"22/09/2020";


    this.dataCurrent =  await this._jobOperationMacSvc.searchJobOperationCurrent(this.model);
    console.log(this.dataCurrent);
    // this.dataPending =  await this._jobOperationMacSvc.searchJobOperationPending(this.model);
    // console.log(this.dataPending);
    // this.dataForward =  await this._jobOperationMacSvc.searchJobOperationForward(this.model);
    // console.log(this.dataForward);

    this.req_date.nativeElement.value = this.model.req_date;
   
  }
}

export class YourValidator {
  static dateVaidator(AC: AbstractControl) {

    if (AC && AC.value && !moment(AC.value, 'YYYY-MM-DD',true).isValid()) {
      return {'dateVaidator': true};
    }
    return null;  
  }

}
