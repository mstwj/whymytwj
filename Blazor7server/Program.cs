using Blazor7server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


//appseting 配置环境..
//Pages 具体页面..
//._Imageport 全局的一个东西...
//App.razor 根主键..
//Routes 路由.
//wwwroot 公共的一些东西...
//Layout 就是公共布局.
//Properties 设置环境..
//BLAZOR 是什么，是MVC的分支，原来是 一个短连接，BLAZOR是一个长连接，为什么要使用BLAZOR呢？ 
//如果我不想 局部刷新，怎么办，只能使用 BLAZOR了.. -- 为什么，如果要解决局部刷新的问题，原来是JS AJOX来解决这个问题。
//现在是使用BLAZOR来解决这个问题了... 或者你使用VUE + 后端，这样的方法，这个就不是我们讨论的范围了..
//MVC的交互，一定是通过JS,AJAX来解决的。
//MVVM就不是这样的了... 这就是使用BLAZOR的原因，他的开发效率肯定比MVC高...
//MVC就是BLAZOR SERVER，就这样去理解，只是 BLAZOR SERVER是长连接，底层可以理解为TCP SINGR连接..--都是在服务端计算，把差异，传送过来，修改DOM -- 
//如果你退出，就会销毁很多东西，比如 TCP连接呀，资源呀等什么东西...
//第2个方式，全部运行在浏览器的沙盒里面...(很多人，非要拿服务器路径？你拿的到嘛？)为什么主播 不愿意呢？（因为这个算C/S模式了..）
//第一次，会加载很多DLL。。。 如果不更新就很麻烦..
//切换会卡，这就是弊端... 这就有2个弊端了... 为什么使用这个BLAZOR，主要就是要替换，JS 不要使用JS 使用C# 就OK了..
//第2个方式：适合什么呢？比如 KFC 麦当劳。。。 第1个方式就适合：工业软件... 厂内工作啥的..如果你不懂HTMLM JS CSS，你做的就是一坨。
//为什么，因为浏览器，就只认识这3个东西..
//.razor 都是组件.. UI就可以理解为组件..
//@page "/fetchdata" -- 可以简单的理解，加了这个，就是一个页面，不加，就是一个组件，可以让别人调用.
//App.razor,就是路由，如果你的razor文件里面加了@pape ,底层就添加为路由的一部分.
//BLAZOR 的路由很简单，你不需要去搞懂。只知道是这样就OK了..
//Blazor 很主要，生命周期...
//所有主键的 基类.. CompontBase,继承了 接口 IComponent,IHandleleEvent,IHandleAfterrender
//只要继承了CompontBase就是一个组件..
//OnInitintzde..
//3大步骤.. 类是没有生命周期的，可是组件有.. 组件我们不能NEW..
//为什么，反正不行。这个你不管...
//注意看：Fetch会渲染2次.. 我自己加了一个Task...
//_Hostcshtml  <component type="typeof(App)" render-mode="ServerPrerendered" /> 注意这里设置的渲染模式.
//可是设置模式..ServerPrerendered的好处就是渲染2次，如果不是这样模式，只渲染1次.. 如果设置为STATIC静态就是最快的.. 不回调..

//app.MapBlazorHub(); 这里就是启动了一个 RRS。。
//app.MapFallbackToPage("/_Host"); 开始了映射..理解为路由..
//_Host这个文件变成一个入口点..
//原来是@layout --> 现在不这样了，现在是 把所有的东西，都写在_Host里面了...
//流程 1 _Host-->App.razor(路由的)-->找到了-->MainLayout
//                                   找不到-->MainLayout

//<NavMenu /> 侧边兰..  @Body分开就是主要东西..
//记住BLAZOR就是MVC...只是他很多转换呀，很多改变呀..

//这个处理不了多标签，比如你开了4个标签，如果第5个，找不到路由，前4个都变成找不到，这就是APP 路由的一个BUG。。。
//这个BUG，你要自己去处理..

//注意;如果你手动敲和点，是完全不一样的，如果是 手动敲，是所有的完全重新来一次，如果是点，只是局部刷新..
//Nav-->张开就是NavMenu.razor组件...

//注意：原来的ASP 程序员，喜欢这样.. http://xxxx?id=199; 比如是这样，现在就不推荐了..因为路由被接管了..
//其实，这里.razor ，也可以，可是我们不要这样去做..
//@Body怎么在底层变成 HTML,这里，我们不要去理解，反正可以这样做，是肯定的...

//NavigationManager --这个类，很主要：是一个跳转的类.. 这里使用这个类，可以做很多事情..
//NavigationManger-->你可以选择是怎么去跳转，跳转又有很多种，1点击一样去跳转，2和原始的一样，使用URL写地址去跳转..

//.cshtml 和 razor有什么区别，可以理解没啥区别，如果非要去说区别，应该就是解析的区别... .razor就是最后变成了DOM元素，CSHTML，的HTML不会变成DOM元素..
//热更新就是个笑话...

//appseting.json -- 配置文件，一般修改不会生效..
//可以在项目上，右键--管理机密文件：打开一个 secret.json的文件，如果又这个文件，系统第一选择是这个..
//你可以看这个文件，是在你自己的硬盘上的，他这个文件不和你的项目一起发布的.. 
//这里，你注意：如果你是开发模式 一般读取的是appsetting.Development.json 
//如果你是 发布模式，才读取appsetting.sjon.. 如何判定是什么模式呢？通过launchSettting.sjon来看..
//如果是development 就是开发模式了..

//内置服务..
//其实内置服务很少的..默认就2个..
//1.是JS 特效。 这里UI已经渲染完成了.. 
//比如一个走马灯：老师将了个例子，比如 Task.Deally-等1秒 添加一个CSS样式，在等1秒 删除1个CSS样式...
//注入很简单，主要是3个模式. 1 Singletion 单例，2 Transient 瞬发(每次都NEW) 3 Scoped 隔离..（NEW一次.每次打开网页就NEW一次.）
//注意BLAZOR就是MVC的一个子集，原来的MVC都可以使用的. BLAZOR 就是接管了,MVC的路由跳转啥的.. 原来的MVC都是还可

//理解组件
//1.占位置主键..<compont tpy="typeof(HeadOutlet)"..这样的... HeadOutlet就是一个组件.
//2.继承主键 @inherit LayoutComponetnBase 比如这样的，通过继承
//3.通过加标签 @page 这也变成了一个组件..变成了网页组件.. @page路由不能空格.
//组件最主要是 重复使用.. 且套使用... 多项目使用..比如你做了一个库..组件的头字符，必须大写..
//.razor 支持HTML 支持C# 支持<STYLE> 可是不是支持<script>
//.razor到底是什么，说白了，就是一个文本..什么都不是..razor不参与编译.-你的逻辑是在.CS里面，你的UI是在.razor里面，为什么微软这样去设计.
//因为如果这样设计你的代码和UI就分开了.. 底层怎么处理，你不要管，发展你就知道.razor 隐形，吧UI编译变成了C#代码..
//很复杂我听不懂， 反正好像是翻译了，变成了什么.G.CS。。 还有 @{} @code{}这2个是完全不一样的..
//注意：这里的_Imports会加到所有的.razor文件的最开头..  可是对CS文件是无效的..
//那如果我CS文件都想要，怎么办呢？在<ItemGrup>这里去加，这样所有的CS文件就都OK了..

//为什么使用blzor ,因为如果是JS HTML 和 CSS，那么你就疯了，因为打包都要过去，如果是.blazor 只要一个DLL，就都有了..
//组件参数，[paramter] 还有一个特殊的attertemer... 这个特别的东西，就是参数不认识的时候，都会以 键值对的方式，保存到里面，这个ATT。。参数一般我们不使用。。。
//这个paramter参数很主要.. MVVM.. 双向绑定..这个例子有，问题是代码不能动。微软这样写的，不要问为什么，反正不要变动就OK了..

//为什么要使用 级联床上.. [CascadingValue...]就是这个，为什么不使用 组件参数呢？因为传递THIS吗？其实我也不太理解..
//CascadingValue组件的主要优势是简化了组件之间的数据传递 ，因为多人开发。单人是不需要的..

//记住一个组件就是 输入/输出...--组件的本质，就是函数。只不过组件包括了UI...--这里老师仔细讲解了 DUMMY这个例子，我反正应该没听懂。

//RenderFragment 模版..
//写到组件中间的任何东西，默认会赋值给 @ChildContent
// @ChildContent写死了。。。
//RenderFrament就是干这个事的，就是填空。挖洞... 老师开始从头捡起了 。。。
//这里就是很复杂了，你可以去接管.. BuildReder..这个函数就是你去接管了DOM。。
//builder.openElement(1,"Div");
//builder.AddContent(2,"我是代码生成的");
//builder.CloseElement();
//原理：MVC -- AJAX把数据给后台，后台处理在返回..--前台得到数据在渲染.. 这就是局部刷新..
//BLAZOR原理：走 WEBSOCKET。不走HTTP协议.. 有心跳的.. 是长连接的..--反正很复杂，只要记住数据小.就OK.
//如果是简单类型.. 组件一般不会刷新。。如果是复杂类型，主键会刷新的..
//这里要脑袋转一下。<h3>@v.Summary</h3>会1 找DIV，2把里面的东西变成其他东西，3关闭这个DIV。。
//每次刷新。ReaderFrament比如刷新...
//这个也是 父类的很主要的继承..--越来越复杂了.. 脑壳大了..
//这里是金华。。 可是很不好理解...
//KEY这个东西，使用的很少。可以不听，就是什么意思呢,动态的时候 下加是OK的，可是 插入0就不OK了，焦点会变，使用要制定这个KEY。。。
//这里有一个很大，很大的问题呀.. 就是BALZOR有很大问题，后台呢？BLAZOR现在，只能去解决前台问题。
//这里BLAZOR怎么去解决后台问题呢?
//BLAZOR不是 前后台分离的东西，如果人家老项目，就很麻烦了..
//try cacch..还是有使用的..
//<ErrorBoundary>
//<ChildContent>
//<Foo4/>
//</ChildContent>
//<ErrorContent>
//</ErrorContent>
//</ErrorBoundary>
//这个是全局的异常...
//我们都使用BLAZOR了，为什么还要使用JS呢？无JS 无CSS？怎么可能？
//老师，现在这里去将JS了。。还要听吗？-- 标签组成文档..
//你千万不要去改BLAZOR维护的HTML。。。 做BLAZOR的时候，能使用JS就使用JS...
//对于个人来说，无JS，都使用组件。。问题是，组件里面都是JS呀...
//如果有频繁的发数据，比如 很快，这样就要考虑使用JS了... 比如 拖动等...
//记住使用JS是 浏览器来跑，和服务器就没关系了..---\
//JSRuntime。。。 这个是微软 内置服务...
//对于JS来说，就2种，1种有返回值的，1种没有返回值的。。 C#调JS接口，JS的函数代码，在JS里面去写，和C#无关..  JS是在浏览器运行的，和服务器也没关系..
//问题，有时候使用JS，是需要销毁的，那么怎么办呢？ 你去继承.. IDisposeAsync 接口，为什么不是 IDispose接口呢？
//比如： Topology: IAsyncDisposable,IDisposbale 这2个接口可以同时继承的.. 如果都继承了. 只执行IasycDisposbale,为什么要销毁呢？因为JS里面 调用了很多东西，比如JS里面开了流，开了 摄像头，这你C#怎么关闭，只能使用JS去销毁了.. 
//这里有个关键问题，就是你去写JS,BLAZOR来调用的时候，你不能直接就去调用，你只能写函数，我来调用，因为，BALZOR的调用时间，JS是不知道的..
//现在BLAZOR 有动态加载的技术.. JSRuntime.InvokeAsync<IJSOBJECt>("xxx","xxx/xxx/xxx.js"); await Module.InvokeVoidAsync("init",xxx,xxxx); 原来是只能写在网页开头..
//为什么只能放在 onAfert,不能放在，INITI里面呢？，因为 INIT里面还没有执行呢？谁执行呀？DOM都没有，页面没有开始渲染呀..
//这是JS的一些，注意点..这个工程就是BLAZOR最基本的东西。。对应老师讲的几个东西..


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(op=>
{
    //如果配置了这个，就先这个..后 在读取. apppsettings.
    //launchset..这个是启动制定的..
    //开启报错..
    op.DetailedErrors = true;
});
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
//这个_HOST就是启动的第一个文件..
app.MapFallbackToPage("/_Host");

app.Run();
