import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { YourValidator } from '../../scan-inprocess/inprocess-search/inprocess-search.component';
import { JobOperationSearchView, JobOperationDetailView, JobOperationDataTotalView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOperationService } from '../../_service/job-operation.service';
import { PrintInprocessDetailComponent } from '../print-inprocess-detail/print-inprocess-detail.component';

@Component({
  selector: 'app-print-inprocess',
  templateUrl: './print-inprocess.component.html',
  styleUrls: ['./print-inprocess.component.scss']
})
export class PrintInprocessComponent implements OnInit {

  @ViewChild('req_date') req_date: ElementRef; 
  
  constructor(
    private _jobOperationMacSvc: JobOperationService,
    private _authSvc: AuthenticationService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    //public dialogRef: MatDialogRef<any>,
  ) { }

  public model: JobOperationSearchView = new JobOperationSearchView();
  public dataCurrent: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  // public dataPending: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  // public dataForward: JobOperationDetailView<JobOperationDataTotalView> = new JobOperationDetailView<JobOperationDataTotalView>();
  
  public user: any; 
  public datePipe = new DatePipe('en-US'); 
  public validationForm: FormGroup;
  //dialogRef : any;

  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser(); 
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
    // this.dataPending.dataTotals  = [];
    // this.dataForward.dataTotals  = [];

    sessionStorage.setItem('jobOperation-reqDate', "");
    sessionStorage.setItem('jobOperation-wcCode', "");
    sessionStorage.setItem('jobOperation-build_type', this.user.branch.entity_code);

    this.model.build_type = this.user.branch.entity_code;
    this.model.wc_code    = "";
    this.model.req_date   = "";


    this.dataCurrent =  await this._jobOperationMacSvc.searchJobOperationCurrent(this.model);
    console.log(this.dataCurrent.dataTotals);
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
    console.log(this.dataCurrent.dataTotals);
    // this.dataPending =  await this._jobOperationMacSvc.searchJobOperationPending(this.model);
    // console.log(this.dataPending);
    // this.dataForward =  await this._jobOperationMacSvc.searchJobOperationForward(this.model);
    // console.log(this.dataForward);

    this.req_date.nativeElement.value = this.model.req_date;
   
  }

  openOrderSummaryDialog(p_build_type: string, p_pdjit_grp: string, p_req_date: string, p_pdjit_grp_desc: string, p_wc_code: string, _isEdit: boolean = false, _index: number = -1) {
    const dialogRef = this._dialog.open(PrintInprocessDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
        build_type: p_build_type,
        pdjit_grp: p_pdjit_grp,
        req_date: p_req_date,
        pdjit_grp_desc: p_pdjit_grp_desc,
        wc_code : p_wc_code,
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
