import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, PageEvent } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanInprocessStockCancelComponent } from '../../scan-inprocess-stock/scan-inprocess-stock-cancel/scan-inprocess-stock-cancel.component';
import { ScanInprocessStockEntryAddComponent } from '../../scan-inprocess-stock/scan-inprocess-stock-entry-add/scan-inprocess-stock-entry-add.component';
import { ScanInprocessStockScanAddComponent } from '../../scan-inprocess-stock/scan-inprocess-stock-scan-add/scan-inprocess-stock-scan-add.component';
import { JobOperationStockView, JobOperationStockSearchView } from '../../_model/job-operation-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanInprocessStockService } from '../../_service/scan-inprocess-stock.service';
import { ScanDefectEntryAddComponent } from '../scan-defect-entry-add/scan-defect-entry-add.component';
import { ScanDefectEntryCancelComponent } from '../scan-defect-entry-cancel/scan-defect-entry-cancel.component';
import { ScanDefectOrderSearchComponent } from '../scan-defect-order-search/scan-defect-order-search.component';
import { ScanDefectScanAddComponent } from '../scan-defect-scan-add/scan-defect-scan-add.component';
import { SummaryDefectComponent } from '../summary-defect/summary-defect.component';

@Component({
  selector: 'app-scan-defect',
  templateUrl: './scan-defect.component.html',
  styleUrls: ['./scan-defect.component.scss']
})
export class ScanDefectComponent implements OnInit {

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _jobSvc: ScanInprocessStockService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: JobOperationStockView = new JobOperationStockView();
  public searchModel: JobOperationStockSearchView = new JobOperationStockSearchView();

  public data: any = {};
  public data_fin: any = {};
  public data_defect: any = {};
  public datas: any = {};
  public data_docdate : any;
  public count_datagroup : any = {};
  public reqDate : any;
  public wclist: any; 
  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    this.reqDate = new Date();
    this.searchModel.req_date = this.reqDate;
    this.searchModel.build_type = this.user.branch.entity_code;
    this.wclist = await this._dll.getDdlWCInprocessStock(this.user.username);
    // console.log(this.wclist);
    if(this.wclist.length >= 1)
    {
      this.searchModel.wc_code = this.wclist[0].key;
      // console.log(this.searchModel.wc_code);
    }
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      req_date: [null, [Validators.required]],
      wc_code: [null, [Validators.required]],
      build_type: [null, [Validators.required]]
    });
  }
  
  async search(event: PageEvent = null) {   
    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data_docdate = this.searchModel.req_date;
    
     
    var datePipe = new DatePipe("en-US");
    this.searchModel.req_date = datePipe.transform(this.searchModel.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;

    console.log(this.searchModel);
    this.data =  await this._jobSvc.searchInProcessPlan(this.searchModel);
    this.data_fin =  await this._jobSvc.searchInProcessDefect(this.searchModel);
    console.log(this.data_fin);
    
    // this.fin_date.nativeElement.value = this.data_findate;
    this.searchModel.req_date = this.data_docdate;
   
}

openScanAdd(p_entity : string ,p_por_no: string ,p_ref_no: string ,p_req_date: string,p_wc_code: string , _index: number = -1)
  {
    console.log(p_entity);
    console.log(p_por_no);
    console.log(p_ref_no);
    console.log(p_req_date);
    console.log(p_wc_code);
    const dialogRef = this._dialog.open(ScanDefectScanAddComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity:p_entity,
        ref_no : p_ref_no,
        req_date : p_req_date,
        wc_code : p_wc_code
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      this.search();
   
    })

  }

  openEntryAdd(p_entity : string ,p_por_no: string ,p_ref_no: string,p_req_date: string ,p_wc_code: string , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanDefectEntryAddComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity:p_entity,
        ref_no : p_ref_no,
        req_date : p_req_date,
        wc_code : p_wc_code
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      this.search();
   
    })

  }

  openCancel(p_entity : string ,p_por_no: string ,p_ref_no: string,p_req_date: string ,p_wc_code: string , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanDefectEntryCancelComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity:p_entity,
        ref_no : p_ref_no,
        req_date : p_req_date,
        wc_code : p_wc_code
       
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      this.search();
      // if (result.length > 0) {
      //   //this.add_prod(result);
      // }
    })
  }

  openSummaryDefect(p_entity : string ,p_por_no: string ,p_ref_no: string,p_req_date: string ,p_wc_code: string , _index: number = -1)
  {
    const dialogRef = this._dialog.open(SummaryDefectComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity:p_entity,
        ref_no : p_ref_no,
        req_date : p_req_date,
        wc_code : p_wc_code
       
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   this.search();
    //   // if (result.length > 0) {
    //   //   //this.add_prod(result);
    //   // }
    // })
  }

  openGetOrderReq()
  {
    const dialogRef = this._dialog.open(ScanDefectOrderSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        wc_code : this.searchModel.wc_code       
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      
      if (result.length > 0) {
        this.add_data(result);
      }
    })
  }

  add_data(datas: any) {
  
    this.searchModel.req_date = datas[0].req_date;
    this.search();
    
  }


close() {
  this._router.navigateByUrl('/app/home');
}

}
