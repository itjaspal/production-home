import { Component, OnInit, ViewChild, ElementRef, Inject } from '@angular/core';
import { PageEvent, MatDialogRef, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOperationService } from '../../_service/job-operation.service';
import { DatePipe } from '@angular/common'
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JobOperationSearchView, JobOperationDetailView, JobOperationDataTotalView } from '../../_model/job-operation';
import * as moment from 'moment';
import { JobOrderSummaryComponent } from '../job-order-summary/job-order-summary.component';

@Component({
  selector: 'app-job-operation',
  templateUrl: './job-operation.component.html',
  styleUrls: ['./job-operation.component.scss']
})
export class JobOperationComponent implements OnInit {

  public model: JobOperationSearchView = new JobOperationSearchView();
  public dataCurrent: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  public dataPending: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  public dataForward: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  
  public user: any; 
  public datePipe = new DatePipe('en-US'); 
  public validationForm: FormGroup;
  

  @ViewChild('req_date') req_date: ElementRef;
  

  constructor(
    private _jobOperationMacSvc: JobOperationService,
    private _authSvc: AuthenticationService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    
   // @Inject(MAT_DIALOG_DATA) public paramData: ViewSpecDrawingParamView
  ) { }

  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser(); 
 
    /*console.log("this.req_date : " + this.req_date.nativeElement.value);
    console.log("this.wc_code : " + this.validationForm.get('wc_code').value);
    console.log("this.build_type : " + this.validationForm.get('build_type').value);
    console.log("this.user.branch.entity_code : " + this.user.branch.entity_code);*/

   /* if ((this.model.build_type == null)||(this.model.build_type == "")||(this.model.wc_code == "")||(this.model.wc_code == null)||(this.model.req_date == "")||(this.model.req_date == null)) {
        this.model.build_type = this.user.branch.entity_code;
        console.log("this.model.build_type is null " + this.user.branch.entity_code);
    }  */

    this.model.build_type = this.user.branch.entity_code;
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
    
    this.model.user_id = this.user.username;
    this.dataCurrent.dataTotals  = [];
    this.dataPending.dataTotals  = [];
    this.dataForward.dataTotals  = [];

    sessionStorage.setItem('jobOperation-reqDate', "");
    sessionStorage.setItem('jobOperation-wcCode', "");
    sessionStorage.setItem('jobOperation-build_type', this.user.branch.entity_code);

    this.model.build_type = this.user.branch.entity_code;
    // this.model.wc_code    = "";
    this.model.req_date   = "";

    console.log(this.user.userWcPrvlgList);
    if(this.user.userWcPrvlgList.length > 1)
    {
      this.model.wc_code = this.user.userWcPrvlgList[0].wc_code;
      
    }
    console.log(this.model.wc_code);

    this.dataCurrent =  await this._jobOperationMacSvc.searchJobOperationCurrent(this.model);
    console.log(this.dataCurrent.dataTotals);
    this.dataPending =  await this._jobOperationMacSvc.searchJobOperationPending(this.model);
    console.log(this.dataPending);
    this.dataForward =  await this._jobOperationMacSvc.searchJobOperationForward(this.model);
    console.log(this.dataForward);
     
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
    this.dataPending.dataTotals  = [];
    this.dataForward.dataTotals  = [];

    sessionStorage.setItem('jobOperation-reqDate', this.req_date.nativeElement.value);
    sessionStorage.setItem('jobOperation-wcCode', this.validationForm.get('wc_code').value);
    sessionStorage.setItem('jobOperation-build_type', this.validationForm.get('build_type').value);
    /*console.log("this.req_date : " + this.req_date.nativeElement.value);
    console.log("this.wc_code : " + this.validationForm.get('wc_code').value);
    console.log("this.build_type : " + this.validationForm.get('build_type').value);*/

    this.model.build_type = this.validationForm.get('build_type').value;
    if (this.validationForm.get('wc_code').value == "0") {
        this.model.wc_code    = "";//"PH4";
    } else {
        this.model.wc_code    = this.validationForm.get('wc_code').value;//"PH4";
    }    
    this.model.req_date   = this.req_date.nativeElement.value;//"22/09/2020";


    this.dataCurrent =  await this._jobOperationMacSvc.searchJobOperationCurrent(this.model);
    console.log(this.dataCurrent.dataTotals);
    this.dataPending =  await this._jobOperationMacSvc.searchJobOperationPending(this.model);
    console.log(this.dataPending);
    this.dataForward =  await this._jobOperationMacSvc.searchJobOperationForward(this.model);
    console.log(this.dataForward);

    this.req_date.nativeElement.value = this.model.req_date;
    //this.validationForm.get('wc_code').setValue(this.model.wc_code);
    //this.validationForm.get('build_type').setValue(this.model.build_type);

  }


  openOrderSummaryDialog(p_build_type: string, p_pdjit_grp: string, p_req_date: string, p_pdjit_grp_desc: string, _isEdit: boolean = false, _index: number = -1) {
    const dialogRef = this._dialog.open(JobOrderSummaryComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
        build_type: p_build_type,
        pdjit_grp: p_pdjit_grp,
        req_date: p_req_date,
        pdjit_grp_desc: p_pdjit_grp_desc,
        isEdit: _isEdit,
       // editItem: _editItem,
        hideSerialNo: true,
        isSaleBed: false
      }
    });
  
    dialogRef.afterClosed().subscribe(result => { 
      if (result) {
        
      }
    });
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
