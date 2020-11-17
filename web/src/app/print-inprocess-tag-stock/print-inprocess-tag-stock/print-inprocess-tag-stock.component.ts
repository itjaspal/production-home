import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { InprocessTagProductSearchComponent } from '../../print-inprocess-tag/inprocess-tag-product-search/inprocess-tag-product-search.component';
import { PrintInProcessTagView, PrintInProcessTagSearchView } from '../../_model/print-inprocess-tag';
import { PrintInProcessTagStockSearchView, PrintInProcessTagStockView, TagProductStockSearchView } from '../../_model/print-inprocess-tag-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { PrintInprocessTagStockService } from '../../_service/print-inprocess-tag-stock.service';
import { InprocessTagStockGroupSearchComponent } from '../inprocess-tag-stock-group-search/inprocess-tag-stock-group-search.component';
import { InprocessTagStockProductSearchComponent } from '../inprocess-tag-stock-product-search/inprocess-tag-stock-product-search.component';

@Component({
  selector: 'app-print-inprocess-tag-stock',
  templateUrl: './print-inprocess-tag-stock.component.html',
  styleUrls: ['./print-inprocess-tag-stock.component.scss']
})
export class PrintInprocessTagStockComponent implements OnInit {

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<any>,
    private _printInprocessSvc: PrintInprocessTagStockService,
    @Inject(MAT_DIALOG_DATA) public data: TagProductStockSearchView
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: PrintInProcessTagStockView = new PrintInProcessTagStockView();
  public searchModel: PrintInProcessTagStockSearchView = new PrintInProcessTagStockSearchView();

  //public data_product: any;
  //public model_scan: JobInProcessScanFinView = new JobInProcessScanFinView();

  public datas: any = {};
  public count = 0;

  //public result: any = {};

  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    

    // this.model = await this._printInprocessSvc.getproductinfo(this.data.bar_code);
    // this.model.req_date = this.data.req_date;
    // this.model.user_id = this.user.username;

  
    console.log(this.model);
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      sub_prod_Code: [null, [Validators.required]],
      group_line: [null, [Validators.required]],
      qty: [null, []],
      description: [null, []]
    });
  }


  async print() {
    console.log(this.model);
    this.datas = await this._printInprocessSvc.PringTagStock(this.model);
    
  }

  openSearchGroupModal(_index: number = -1)
  {
    
    const dialogRef2 = this._dialog.open(InprocessTagStockGroupSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        req_date: this.data.req_date,
        entity : this.data.entity,
        wc_code : this.data.wc_code,
        por_no : this.data.por_no,
        ref_no : this.data.ref_no,
      }

    });

    dialogRef2.afterClosed().subscribe(result => {
      if (result.length > 0) {
        this.add_prod(result);
      }
      
    })
  }

  openSearchProductModal(_index: number = -1)
  {
    
    const dialogRef2 = this._dialog.open(InprocessTagStockProductSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        req_date: this.data.req_date,
        entity : this.data.entity,
        wc_code : this.data.wc_code,
        por_no : this.data.por_no,
        ref_no : this.data.ref_no,
        group_line : this.searchModel.group_line
      }

    });

    dialogRef2.afterClosed().subscribe(result => {
      if (result.length > 0) {
        this.add_prod(result);
      }
      
    })
  }

  add_prod(datas: any) {

    console.log(datas);
    this.searchModel.sub_prod_code = datas[0].bar_code;
    // this.model.prod_code = datas[0].prod_code;
    // this.model.prod_name = datas[0].prod_name;
    // this.model.design_name = datas[0].design_name;
    // this.model.size_name = datas[0].size_name;
    
  }  


  close_print() {
    //window.history.back();
    this.dialogRef.close();
    // this._router.navigateByUrl('/app/taginproc/inprocserach/' + this._actRoute.snapshot.params.req_date + '/' + this._actRoute.snapshot.params.wc_code);
  }

}
