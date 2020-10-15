import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { OrderSummaryParamView } from '../../_model/job-operation';
import { PrintInProcessTagView, PrintInProcessTagSearchView, TagProductSearchView } from '../../_model/print-inprocess-tag';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { PrintInprocessTagService } from '../../_service/print-inprocess-tag.service';
import { ScanInprocessService } from '../../_service/scan-inprocess.service';
import { InprocessTagProductSearchComponent } from '../inprocess-tag-product-search/inprocess-tag-product-search.component';

@Component({
  selector: 'app-print-inprocess-tag',
  templateUrl: './print-inprocess-tag.component.html',
  styleUrls: ['./print-inprocess-tag.component.scss']
})
export class PrintInprocessTagComponent implements OnInit {
  //dialogRef2 : any;

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<any>,
    private _printInprocessSvc: PrintInprocessTagService,
    @Inject(MAT_DIALOG_DATA) public data: TagProductSearchView
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: PrintInProcessTagView = new PrintInProcessTagView();
  public searchModel: PrintInProcessTagSearchView = new PrintInProcessTagSearchView();

  //public data_product: any;
  //public model_scan: JobInProcessScanFinView = new JobInProcessScanFinView();

  public datas: any = {};
  public count = 0;

  //public result: any = {};

  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    this.searchModel.bar_code = this.data.bar_code;

    this.model = await this._printInprocessSvc.getproductinfo(this.data.bar_code);
    this.model.req_date = this.data.req_date;
    this.model.user_id = this.user.username;

  
    console.log(this.model);
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      bar_code: [null, [Validators.required]],
      qty: [null, []],
      description: [null, []]
    });
  }


  async print() {
    console.log(this.model);
    // var datePipe = new DatePipe("en-US");
    // this.searchModel.wc_code = this._actRoute.snapshot.params.wc_code;
    // this.searchModel.req_date = datePipe.transform(this._actRoute.snapshot.params.req_date, 'dd/MM/yyyy').toString();
    // this.searchModel.user_id = this.user.username;
    // this.searchModel.build_type = this.user.branch.entity_code;
    // this.searchModel.pdjit_grp = this._actRoute.snapshot.params.pdjit_grp;
      
    // this.datas = await this._jobInprocessSvc.searchentryadd(this.searchModel);

    // this.add(this.datas);
    // this.searchModel.bar_code = "";
    // this.searchModel.qty = 0;

  }

  // add(datas: any) {

  //   let newProd: JobInProcessScanView = new JobInProcessScanView();
  //   newProd.prod_code = datas.prod_code;
  //   newProd.prod_name = datas.prod_name;
  //   newProd.qty = datas.qty;


  //   this.model_scan.datas.push(newProd);

  //   // Group By Product
  //   var result = [];
  //   this.model_scan.datas.forEach(function (a) {
  //     if (!this[a.prod_code] && !this[a.prod_name]) {
  //       this[a.prod_code] = { prod_code: a.prod_code, prod_name: a.prod_name, qty: 0 };
  //       result.push(this[a.prod_code]);
  //     }
  //     this[a.prod_code].qty += a.qty;

  //   }, Object.create(null));

  //   this.model_scan.datas = result;
  //   //console.log(result)
  //   console.log(this.model_scan.datas);


  // }


  openSearchProductModal(_index: number = -1)
  {
    const dialogRef2 = this._dialog.open(InprocessTagProductSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
         req_date: this.data.req_date,
         pdjit_grp:this.data.pdjit_grp,
         entity : this.data.entity
      }

    });

    dialogRef2.afterClosed().subscribe(result => {
      // if (result.length > 0) {
      //   this.add_prod(result);
      // }
      if (result) {
        
      }
    })
  }

  add_prod(datas: any) {

    console.log(datas);
    // this.searchModel.bar_code = datas[0].bar_code;
    // this.searchModel.qty = 1;
  }  


  close_print() {
    //window.history.back();
    this.dialogRef.close();
    // this._router.navigateByUrl('/app/taginproc/inprocserach/' + this._actRoute.snapshot.params.req_date + '/' + this._actRoute.snapshot.params.wc_code);
  }

}
