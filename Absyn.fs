(* File MicroC/Absyn.fs
   Abstract syntax of micro-C, an imperative language.
   sestoft@itu.dk 2009-09-25

   Must precede Interp.fs, Comp.fs and Contcomp.fs in Solution Explorer
 *)

module Absyn

// 基本类型
// 注意，数组、指针是递归类型
// 这里没有函数类型，注意与上次课的 MicroML 对比
type typ =
  | TypI                             (* Type int                    *)
  | TypC                             (* Type char                   *)
  | TypF                             (* Type float32                *)
  | TypD                             (* Type double                 *)
  | TypL                             (* Type long                   *)
  | TypS                             (* Type string                 *)
  | TypB                             (* Type boolean                *)
  | TypA of typ * int option         (* Array type                  *)
  | TypP of typ                      (* Pointer type                *)

//of后面是参数列表                                                              
and expr =                           // 表达式，右值         
  | PreInc of access                 (* ++i or ++a[e]               *)
  | PreDec of access                 (* --i or --a[e]               *)                                       
  | Access of access                 (* x    or  *p    or  a[e]     *) //访问左值（右值）
  | Assign of access * expr          (* x=e  or  *p=e  or  a[e]=e   *)
  | AssignPrim of string *access * expr      (* x+=e or  *p+=e or  a[e]+=e  *)
  | Addr of access                   (* &x   or  &*p   or  &a[e]    *)
  | CstI of int                      (* Constant                    *)
  | CstF of float
  | CstD of double
  | CstS of string
  | Prim1 of string * expr           (* Unary primitive operator    *)
  | Prim2 of string * expr * expr    (* Binary primitive operator   *)
  | Prim3 of expr * expr * expr      (*  ? ;                        *)
  | Andalso of expr * expr           (* Sequential and              *)
  | Orelse of expr * expr            (* Sequential or               *)
  | Call of string * expr list       (* Function call f(...)        *)
                                                                   
and access =                         //左值，存储的位置                                            
  | AccVar of string                 (* Variable access        x    *) 
  | AccDeref of expr                 (* Pointer dereferencing  *p   *)
  | AccIndex of access * expr        (* Array indexing         a[e] *)

//基本语句定义                                                       
and stmt =                                                         
  | Switch of expr * stmt list
  | Case of expr * stmt
  | Default of stmt 
  | DoUntil of stmt * expr
  | DoWhile of stmt * expr
  | ForIn of access * expr * expr * expr * stmt 
  | If of expr * stmt * stmt         (* Conditional                 *)
  | For of expr * expr * expr * stmt (* for loop                    *)
  | While of expr * stmt             (* While loop                  *)
  | Expr of expr                     (* Expression statement   e;   *)
  | Return of expr option            (* Return from method          *)
  | Block of stmtordec list          (* Block: grouping and scope   *)
  // 语句块内部，可以是变量声明 或语句的列表                                                              

and stmtordec =                                                    
  | Dec of typ * string              (* Local variable declaration  *)
  | DecAndAssign of typ * string * expr(*declaration and assign     *)
  | Stmt of stmt                     (* A statement                 *)

// 顶级声明 可以是函数声明或变量声明
and topdec = 
  | Fundec of typ option * string * (typ * string) list * stmt
  | Vardec of typ * string
  | VardecAndAssign of typ * string * expr

// 程序是顶级声明的列表
and program = 
  | Prog of topdec list
