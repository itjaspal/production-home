import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, PageEvent } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { SummaryDefectComponent } from '../../scan-defect/summary-defect/summary-defect.component';
import { JobOperationStockView, JobOperationStockSearchView } from '../../_model/job-operation-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { JobOperationStockService } from '../../_service/job-operation-stock.service';
import { JobOperationService } from '../../_service/job-operation.service';
import { MessageService } from '../../_service/message.service';
import { JobOrderDetailComponent } from '../job-order-detail/job-order-detail.component';
import { PorDetailComponent } from '../por-detail/por-detail.component';
import { ProductDetailComponent } from '../product-detail/product-detail.component';
import { ProductGroupDetailComponent } from '../product-group-detail/product-group-detail.component';
import { ProductionTrackingStockComponent } from '../production-tracking-stock/production-tracking-stock.component';

@Component({
  selector: 'app-job-operation-stock',
  templateUrl: './job-operation-stock.component.html',
  styleUrls: ['./job-operation-stock.component.scss']
})
export class JobOperationStockComponent implements OnInit {

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _jobSvc: JobOperationStockService,
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
  public wc_code : any;

  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    this.reqDate = new Date();
    this.searchModel.req_date = this.reqDate;

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

    // console.log(this.searchModel);
    this.data =  await this._jobSvc.searchDataPlan(this.searchModel);
    this.data_fin =  await this._jobSvc.searchDataFin(this.searchModel);
    this.data_defect =  await this._jobSvc.searchDataDefect(this.searchModel);
    
    // this.fin_date.nativeElement.value = this.data_findate;
    this.searchModel.req_date = this.data_docdate;
   
}

openPorDetail(p_entity : string ,p_por_no: string  , _index: number = -1)
  {
    console.log(p_por_no);
    const dialogRef = this._dialog.open(PorDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity_code:p_entity,
        isSaleBed : true
      }

    });

  }

  openMorDetail(p_entity : string ,p_por_no: string  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(PorDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity_code:p_entity,
        isSaleBed : false
      }

    });

  }

  openProductDetail(p_entity : string ,p_por_no: string  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ProductDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity_code:p_entity
       
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result.length > 0) {
    //     //this.add_prod(result);
    //   }
    // })
  }

  openProductionTracking(p_entity : string ,p_por_no: string,p_ref_no: string , p_req_date : string , _index: number = -1)
  {
    console.log(p_por_no);
    console.log(p_ref_no);
    console.log(this.searchModel.req_date);
    console.log(this.wc_code);
    const dialogRef = this._dialog.open(ProductionTrackingStockComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: p_por_no,
        entity_code:p_entity,
        ref_no :p_ref_no,
        req_date : p_req_date,
        wc_code : this.wc_code,
        user_id : this.user.username
        // build_type : this.user.branch.entity_code,
       
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result.length > 0) {
    //     //this.add_prod(result);
    //   }
    // })
  }


  openSummaryDefect(p_entity : string ,p_por_no: string ,p_ref_no: string,p_req_date: string  , _index: number = -1)
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
        wc_code : this.wc_code,
       
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   this.search();
    //   // if (result.length > 0) {
    //   //   //this.add_prod(result);
    //   // }
    // })
  }

close() {
  this._router.navigateByUrl('/app/home');
}


}
