IF OBJECT_ID('[sm].[_sp_generate_cshtml5_links]', 'P') IS NOT NULL
	DROP PROCEDURE [sm].[_sp_generate_cshtml5_links]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*

select * from t_order_request
sm._sp_generate_cshtml5_links 'd_work','d_work_group'


*/

CREATE PROCEDURE [sm].[_sp_generate_cshtml5_links]
	@table_name nvarchar(100),
	@table_name2 nvarchar(100)
as
	set nocount on

	declare
		@name nvarchar(100) = substring(@table_name, 3, len(@table_name) -2),
		@prefix nvarchar(10) = left(@table_name, 2),
		@crlf nvarchar(4) = char(13) + char(10),
		@namespaceNet		nvarchar(255) 

	declare
		@camel_case_name	nvarchar(255) = sm._fn_to_camel_case(@name),
		@camel_case_many_name	nvarchar(255) = [sm].[_fn_many_suffix](sm._fn_to_camel_case(@name))

	declare
		@upper_case_name	nvarchar(255) = sm._fn_first_upper(@camel_case_name)
	,	@prefixNet			nvarchar(255) = sm._fn_first_upper(sm._fn_to_camel_case(@prefix))	


		select @namespaceNet =  [sm].[_fn_net_namespace](@table_name)
			
------------------------------------------------------------------------------------------------------
-- проверки регистрации таблицы
------------------------------------------------------------------------------------------------------
	--SELECT @table_id = id FROM s_Table WHERE [name] = @table_name

	--IF @table_id IS NULL BEGIN
	--	PRINT '<row id="' + cast(newid() as nvarchar(100)) + '" name="' + @table_name + '" caption="!!!Описание таблицы!!!" is_mergeable="0" for_surcharge="0" archive="0" is_main_table_of_frame="1" proc_name="up_' + @name + '" view_name="v' + @table_name + '" />>'
	--	EXEC upu_raise_error  @error_code = 300000, @message_marker = 'Таблица не зарегистрирована в s_Table'
	--	RETURN
	--END
------------------------------------------------------------------------------------------------------
-- Collect columns
------------------------------------------------------------------------------------------------------
	declare @t table ( n int, name nvarchar(100), fulltype nvarchar(200), type nvarchar(100), isnullable int )
	insert @t (n, name, fulltype, type, isnullable)
	select 
		row_number() over(order by (select null)),
		col.name,
		tp.name,-- + case when tp.name in ('char', 'nvarchar', 'nchar', 'nvarchar') then '(' + ltrim(str(col.length)) + ')' else '' end, 
		tp.name,
		col.isnullable
	from 
		[syscolumns] col
		join [sysobjects] obj on col.id = obj.id
		join [systypes] tp on tp.xusertype = col.xusertype
	where obj.name = @table_name


	declare @t_children table(name nvarchar(200), camel_case_name nvarchar(200), upper_case_name nvarchar(200),upper_table_name nvarchar(200))
	insert @t_children
	select 
		t.name,
		camel_case_name = sm._fn_to_camel_case(substring(t.name, 3, len(t.name) -2)),
		upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(substring(t.name, 3, len(t.name) -2))),
		upper_table_name = sm._fn_first_upper(sm._fn_to_camel_case(t.name))
	from
		sys.tables t
		join sys.foreign_keys fk on fk.parent_object_id = t.object_id
		join sys.foreign_key_columns fkc on fkc.constraint_object_id = fk.object_id
		join sys.all_columns c on c.column_id = fkc.parent_column_id AND c.object_id = fkc.parent_object_id
		join sys.all_columns  refC on refC.column_id = fkc.referenced_column_id AND refC.object_id = fkc.referenced_object_id
		join sys.tables refT on reft.object_id = refC.object_id
	where
		refT.name = @table_name	

		

	declare @maxn int
	select @maxn = max( n ) from @t
------------------------------------------------------------------------------------------------------
-- Блок проверки регистрации таблицы -- КОНЕЦ
------------------------------------------------------------------------------------------------------
	declare @lines table( line nvarchar(max), id int identity(1,1) )
------------------------------------------------------------------------------------------------------
-- Business collections - BEGIN
------------------------------------------------------------------------------------------------------
	insert @lines ( line ) select ''
				
		union all	select	'
--------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'Page.xaml
------------------------------------------------------------------------------------------------------

                                <tnomer:XListBox x:Name="WorkGroupList" Text="Группы работ" 
                                     DisplayMemberPath="Name" Items="{Binding workGroups, Mode=TwoWay}"
                                     RequestList="WorkGroupList_RequestList">
                                    <tnomer:XListBox.DialogFilter>
                                        <WrapPanel >
                                            <tnomer:XTextBox x:Name="FilterWorkGroupListNameTextBox" Text="Имя" 
                                                 TextWrapping="NoWrap" AcceptsReturn="False" 
                                                 ValueChanged="FilterWorkGroupListNameTextBox_ValueChanged"
                                                 />
                                        </WrapPanel>

                                    </tnomer:XListBox.DialogFilter>
                                </tnomer:XListBox>

				'
		union all 	select	char(13) + char(10) 
						+ case 
									when fulltype in ('int','bigint') and right(name,3) ='_id' then
		'						<tnomer:XComboBox x:Name="' + upper_case_root_name + 'ComboBox" Text="' + name + '" HorizontalAlignment="Stretch"
									SelectedValue="{Binding ' + sm._fn_to_camel_case(name) + ', Mode=TwoWay}"
									LookupRefreshRequest="' + upper_case_root_name + 'ComboBox_LookupRefreshRequest"
									DisplayMemberPath="Name"  SelectedValuePath="id" />'
									when fulltype in ('int','byte','float','numeric','money','decimal') then
		'						<tnomer:XNumericBox x:Name="' + upper_case_name + 'NumericBox" Text="' + name + '" 
									Value="{Binding ' + sm._fn_to_camel_case(name) + ', Mode=TwoWay}" Min="0" Max="420"
									HorizontalAlignment="Stretch"/>'
									else
		'						<tnomer:XTextBox x:Name="' + upper_case_name + 'TextBox" Text="' + name + '"
									Value="{Binding ' + sm._fn_to_camel_case(name) + ', Mode=TwoWay}"
									TextWrapping="NoWrap" AcceptsReturn="False" />'
									end
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where
								name not in( 'id') 
								and 
								left(name,2) <> 's_' 
						) t
					where 
						-- не обрабатываемые столбцы
						name not in ('id', 's_archive', 's_version')
		union all	select	
'							</StackPanel>
						</ScrollViewer>
					</TabItem>
				</TabControl>
			</Grid>
        </tnomer:PopupContainer.BasePanel>
    </tnomer:PopupContainer>
</Page>
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'Page.xaml.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Tnomer.Remontica.Html5.Models.' + @namespaceNet + ';
using Tnomer.Remontica.Html5.Repository;
using Tnomer.Web.Html5;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tnomer.Remontica.Html5.' + @namespaceNet + '
{	
	public partial class ' + @upper_case_name + 'Page : Page
	{
		String includeParameters = "";

        public event RoutedEventHandler OnSave;
        public event RoutedEventHandler OnCancel;

	    public ' + @upper_case_name + 'Page()
	    {
			InitializeComponent();
			TabControl1.SelectedIndex = 0;
		}	

		public async Task SetModelAsync(' + @upper_case_name + 'Model model)
		{	
			_model = model;

			//  load lookups'
		union all	select	
'	        var task' + upper_case_root_name + ' = Refresh' + upper_case_root_name + 'LookupAsync();	'
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) g
		union all	select	'
			//  await lookups	'
		union all	select '	            await task' + upper_case_root_name + ';'	
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t
		union all	select	'
			//  add/edit	
			if (model.id > 0)	
			{
				MainStripPanel.Text = "Изменение <????>";
			}
			else
			{	
				MainStripPanel.Text = "Добавление ????";	'
		union all	select	'	                
				if (model.' + camel_case_name + ' == 0)
					model.' + camel_case_name + ' = (await RemoteRepository.' + @namespaceNet + '.' + upper_case_root_name + '.Get()).data.' + sm._fn_many_suffix(camel_case_root_name) + '.OrderBy(t => t.Name).First().id;'
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								camel_case_root_name =  left(sm._fn_to_camel_case(name), len(sm._fn_to_camel_case(name)) -2),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t
		union all	select	'	            }

	        //  set model
	        DataContext = model;
	    }

		' + @upper_case_name + 'Model _model;
	    public ' + @upper_case_name + 'Model Model
	    {
	        get => _model;
	    }

        public static ' + @upper_case_name + 'Model CreateModel()
        {
	        return new ' + @upper_case_name + 'Model()
	        {'
		union all	select 
'				' + [sm].[_fn_many_suffix](q.camel_case_name) + ' = new ObservableCollection<' + q.upper_case_name + 'Model>(),'
					from
						@t_children q	

		union all	select	
'			};
        }

        public async Task NewModelAsync()
        {
            var model = CreateModel();
            await SetModelAsync(model);
        }

        public async Task LoadModelAsync(Int32 id)
        {
            var filterParams = new FilterParameters(includeParameters);
            var model = (await RemoteRepository.' + @namespaceNet + '.' + @upper_case_name + '.Get(id, filterParams)).data.' + @camel_case_name + ';
            await SetModelAsync(model);
        }
        //*******************************************************************
        //  Save
        //*******************************************************************
        public async Task SaveAsync()
        {
            FilterParameters filterParams = new FilterParameters(includeParameters);

            if (Model.id == 0)
                await RemoteRepository.' + @namespaceNet + '.' + @upper_case_name + '.Insert(Model, filterParams);
            else
                await RemoteRepository.' + @namespaceNet + '.' + @upper_case_name + '.Update(Model.id, Model, filterParams);
        }
        //*******************************************************************
        //  Remove UI
        //*******************************************************************
        public static async Task<Boolean> RemoveUIAsync(' + @upper_case_name + 'Model model)
        {
            try
            {
                if (model != null && await MessageBoxDialog.ShowAsync("Удалить ??? [" + model.Name + "] ?", "Remontica", WarningLevel.Exclamation, Buttons.Ok) == Buttons.Ok)
                {
                    await RemoteRepository.' + @namespaceNet + '.' + @upper_case_name + '.Remove(model.id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                await MessageBoxDialog.ShowAsync(ex.Message);
            }

            return false;
        }
	    //  ******************************************************
	    //  Loading lists
	    //  ******************************************************'
		union all	select	
'		private async Task Refresh' + upper_case_root_name + 'LookupAsync()
	    {
	        try
	        {
	            ' + upper_case_root_name + 'ComboBox.ResetLookupError();
	            ' + upper_case_root_name + 'ComboBox.ItemsSource = (await RemoteRepository.' + @namespaceNet + '.' + upper_case_root_name + '.Get()).data.' + sm._fn_many_suffix(camel_case_root_name) + '.OrderBy(t => t.Name);
	        }
	        catch (Exception ex)
	        {
	            ' + upper_case_root_name + 'ComboBox.SetLookupError(ex.Message);
	        }
	    }
'
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								camel_case_root_name =  left(sm._fn_to_camel_case(name), len(sm._fn_to_camel_case(name)) -2),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t
		union all	select	
'		//  ******************************************************	
	    //  Controls events
	    //  ******************************************************'
		union all	select	'
		private void ' + upper_case_root_name + 'ComboBox_LookupRefreshRequest(object sender, RoutedEventArgs e)
		{
			Refresh' + upper_case_root_name + 'LookupAsync();
		}
' 
					from 
						(	
							select *,
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t
		union all	select	
'	
        private void OnPopupCancel(object sender, RoutedEventArgs e)
        {
            PopupContainer1.PopupPanel = null;
            PopupContainer1.PopupPanelVisibility = Visibility.Collapsed;
        }

        //*******************************************************************
        //  Editing item
        //*******************************************************************
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke(this, new RoutedEventArgs());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            OnSave?.Invoke(this, new RoutedEventArgs());
        }
	}	
}
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'ListPage.xaml
------------------------------------------------------------------------------------------------------
<Page
    x:Class="Tnomer.Remontica.Html5.' + @namespaceNet + '.' + @upper_case_name + 'ListPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:Tnomer.Remontica.Html5.' + @namespaceNet + '"
    xmlns:tnomer="clr-namespace:Tnomer.Web.Html5.Controls;assembly=Tnomer.Web.Html5"
    xmlns:' + @namespaceNet + '="clr-namespace:Tnomer.Remontica.Html5.' + @namespaceNet + '">

    <Grid>
        <tnomer:PopupContainer x:Name="PopupContainer1">
            <tnomer:PopupContainer.BasePanel>
				<Grid>
					<Grid.RowDefinitions>
						<RowDefinition Height="Auto" />
						<RowDefinition />
					</Grid.RowDefinitions>
					<Grid.ColumnDefinitions>
						<ColumnDefinition />
					</Grid.ColumnDefinitions>

					<tnomer:StripPanel x:Name="StripPanel1" Icon= "&#xE7EE;" Text="?????" Grid.Row="0"
									   SearchPanelVisibility="Visible" FilterButtonVisibility="Visible"
									   SearchTextChanged="StripPanel1_SearchTextChanged"
									   FilterPanelVisibilityChanged="StripPanel1_FilterPanelVisibilityChanged"
									   AddButtonVisibility="Visible" AddButtonClick="StripPanel1_AddButtonClick"
									   EditButtonVisibility="Visible" EditButtonClick="StripPanel1_EditButtonClick"
									   RemoveButtonVisibility="Visible" RemoveButtonClick="StripPanel1_RemoveButtonClick"
									   >
						<tnomer:StripPanel.FilterPanel>
							<Grid>
								<Grid.ColumnDefinitions>
									<ColumnDefinition />
									<ColumnDefinition Width="Auto"/>
								</Grid.ColumnDefinitions>
								<WrapPanel Grid.Column="1">
		'
		union all	select	
'									<tnomer:XComboBox x:Name="Filter' + upper_case_root_name + 'ComboBox" Text="' + upper_case_root_name + '" 
										MaxWidth="400"
										SelectedValuePath="id" DisplayMemberPath="Name" AcceptNull="True"
										TextAlignment = "Right"
										SelectionChanged="FilterComboBox_SelectionChanged"
									/>
'
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t
		union all	select	
'								</WrapPanel>
							</Grid>	
						</tnomer:StripPanel.FilterPanel>
					</tnomer:StripPanel>
                    <tnomer:DataGridNavigator x:Name="DataGridNavigator1" Grid.Row="1" OnDataRequest="DataGridNavigator1_OnDataRequest">
                        <tnomer:DataGridNavigator.ItemsControl>
							<tnomer:DataGrid x:Name="DataGrid1"
											CanEdit="False" CanInsert="False" CanRemove="False"
											SelectionChanged="DataGrid1_SelectionChanged"
											RefreshButtonClick="DataGrid1_RefreshButtonClick"
											>
								<tnomer:DataGrid.Columns>	
									<DataGridTemplateColumn Header=""  >
										<DataGridTemplateColumn.HeaderTemplate>
											<DataTemplate>
												<tnomer:SortedButton Text="№" MinWidth="100" Tag="id"
													Height="30" TextAlignment="Center" Click="SortedButton_Click"/>
											</DataTemplate>
										</DataGridTemplateColumn.HeaderTemplate>
										<DataGridTemplateColumn.CellTemplate>
											<DataTemplate>
												<TextBlock Text="{Binding id}" Padding="5" TextAlignment="Right"/>
											</DataTemplate>
										</DataGridTemplateColumn.CellTemplate>
									</DataGridTemplateColumn>

'
		union all	select	
	'								<DataGridTemplateColumn Header=""  >
										<DataGridTemplateColumn.HeaderTemplate>
											<DataTemplate>
												<tnomer:SortedButton Text="' + camel_case_name + '" MinWidth="100" Tag="' + camel_case_name + '"
													Height="30" TextAlignment="Center" Click="SortedButton_Click"/>
											</DataTemplate>
										</DataGridTemplateColumn.HeaderTemplate>
										<DataGridTemplateColumn.CellTemplate>
											<DataTemplate>
												<TextBlock Text="{Binding ' + camel_case_name + '}" Padding="5" TextAlignment="Left"/>
											</DataTemplate>
										</DataGridTemplateColumn.CellTemplate>
									</DataGridTemplateColumn>
' 
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t
							where
								name not in( 'id') 
								and 
								left(name,2) <> 's_' 
						) t
		union all	select '
                                </tnomer:DataGrid.Columns>
                            </tnomer:DataGrid>
                        </tnomer:DataGridNavigator.ItemsControl>
                    </tnomer:DataGridNavigator>
                </Grid>
            </tnomer:PopupContainer.BasePanel>
        </tnomer:PopupContainer>
    </Grid>
</Page>
'
		union all	select	'
------------------------------------------------------------------------------------------------------
-- ' + @upper_case_name + 'ListPage.xaml.cs
------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tnomer.Remontica.Html5.Models.' + @namespaceNet + ';
using Tnomer.Remontica.Html5.Repository;
using Tnomer.Remontica.Html5.Repository.' + @namespaceNet + ';
using Tnomer.Web.Html5;
using Tnomer.Web.Html5.Controls;
using Tnomer.Web.Html5.Components;
using Tnomer.Web.Html5.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tnomer.Remontica.Html5.' + @namespaceNet + '
{
    public partial class ' + @upper_case_name + 'ListPage : Page
    {
        String includeParameters = "";

        public ' + @upper_case_name + 'ListPage()
        {
            InitializeComponent();
            RequestRefresh();

			// Filters
			RefreshFiltersAsync();
 		}

        //*******************************************************************
        //  List
        //*******************************************************************
        private void RequestRefreshLazy()
        {
            _ListRefreshLazyExecutor.SetHandler(RefreshAsync, 1);
        }
        private void RequestRefresh()
        {
            _ListRefreshLazyExecutor.SetHandler(RefreshAsync, 0);
        }

        LazyExecutor _ListRefreshLazyExecutor = new LazyExecutor();
	    private async Task RefreshAsync()
	    {
	        try
	        {
	            DataGrid1.IsBusy = true;

	            int i;
	            var pars = new ' + @upper_case_name + 'Parameters();
'
		union all	select	'
	            if (Int32.TryParse(Filter' + upper_case_root_name + 'ComboBox.SelectedValue?.ToString(), out i))
	                pars.' + upper_case_name + ' = i;
' 
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								camel_case_root_name =  left(sm._fn_to_camel_case(name), len(sm._fn_to_camel_case(name)) -2),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t

		union all	select '
				pars.SearchText = StripPanel1.SearchText;

				// sorting
                if (_lastSortedButton?.State == SortedButtonState.Ascending)
                    pars.OrderBy = _lastSortedButton.Tag as string;
                else if (_lastSortedButton?.State == SortedButtonState.Descending)
                    pars.OrderBy = "-" + _lastSortedButton.Tag as string;

                // paging
                pars.Skip = Convert.ToInt32(DataGridNavigator1.Skip);
                pars.Top = DataGridNavigator1.RowCount;


                var model = await (RemoteRepository.' + @namespaceNet + '.' + @upper_case_name + '.Get(pars));
                DataGridNavigator1.ItemsSource = model.data.' + @camel_case_many_name + ';
            }
            catch (Exception ex)
            {
                DataGrid1.SetError(ex.Message);
            }
            finally
            {
                DataGrid1.IsBusy = false;
            }
        }
        //*******************************************************************
        //  Filtering
        //*******************************************************************
        private void StripPanel1_SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            RequestRefreshLazy();
        }
        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RequestRefresh();
        }
        //*******************************************************************
        //  Loading lookups
        //*******************************************************************
        Boolean IsFilterLoaded;
        private void StripPanel1_FilterPanelVisibilityChanged(object sender, RoutedEventArgs e)
        {
            if(!IsFilterLoaded) 
                RefreshFiltersAsync();
        }

        private async Task RefreshFiltersAsync()
        {
            if (!IsFilterLoaded)
            {'
		union all	select 
'	            RefreshFilter' + upper_case_root_name  + 'Async();'
					from 
						(	
							select *,
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t
		union all	select 
'
                IsFilterLoaded = true;
            }
        }
		'
		union all	select	'	        private void Filter' + upper_case_root_name + 'ComboBox_LookupRefreshRequest(object sender, RoutedEventArgs e)' + char(13) + char(10) +
							'	        {' + char(13) + char(10) +
							'	            RefreshFilter' + upper_case_root_name + 'Async();' + char(13) + char(10) +
							'	        }' + char(13) + char(10) +
							'	        private async Task RefreshFilter' + upper_case_root_name + 'Async()' + char(13) + char(10) +
							'	        {' + char(13) + char(10) +
							'	            try' + char(13) + char(10) +
							'	            {' + char(13) + char(10) +
							'	                var model = await RemoteRepository.' + @namespaceNet + '.' + upper_case_root_name + '.Get();' + char(13) + char(10) +
							'' + char(13) + char(10) +
							'	                Filter' + upper_case_root_name + 'ComboBox.ItemsSource = model.data.' + sm._fn_many_suffix( camel_case_root_name)  +'.OrderBy(t => t.Name);' + char(13) + char(10) +
							'	                Filter' + upper_case_root_name + 'ComboBox.ResetLookupError();' + char(13) + char(10) +
							'	            }' + char(13) + char(10) +
							'	            catch (Exception ex)' + char(13) + char(10) +
							'	            {' + char(13) + char(10) +
							'	                Filter' + upper_case_root_name + 'ComboBox.SetLookupError(ex.Message);' + char(13) + char(10) +
							'	            }' + char(13) + char(10) +
							'	        }' + char(13) + char(10) 
					from 
						(	
							select *,
								camel_case_name = sm._fn_to_camel_case(name),
								camel_case_root_name =  left(sm._fn_to_camel_case(name), len(sm._fn_to_camel_case(name)) -2),
								upper_case_name = sm._fn_first_upper(sm._fn_to_camel_case(name)),
								upper_case_root_name = substring(sm._fn_first_upper(sm._fn_to_camel_case(name)),1,len(sm._fn_first_upper(sm._fn_to_camel_case(name))) -2) 
							from 
								@t 
							where 
								fulltype in ('int','bigint') and right(name,3) ='_id'
						) t
		union all	select 
'       //*******************************************************************
	    //  Editing list
	    //*******************************************************************
	    private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
	    {
	        StripPanel1.EditButtonEnabled = (e.AddedItems.Count > 0);
	        StripPanel1.RemoveButtonEnabled = StripPanel1.EditButtonEnabled;
	    }

	    private async void StripPanel1_AddButtonClick(object sender, RoutedEventArgs e)
	    {
	        try
	        {
	            var page = new ' + @upper_case_name + 'Page();
                page.OnCancel += OnCancel;
                page.OnSave += OnSave;

	            await page.NewModelAsync();

                PopupContainer1.PopupPanel = page;
                PopupContainer1.PopupPanelVisibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                PopupContainer1.PopupPanelVisibility = Visibility.Collapsed;
                await MessageBoxDialog.ShowAsync(ex.Message);
	        }
	    }

	    private async void StripPanel1_EditButtonClick(object sender, RoutedEventArgs e)
	    {
	        try
	        {
                var model = (DataGrid1.SelectedItem as ' + @upper_case_name + 'Model);
                if (model == null) return;

	            var page = new ' + @upper_case_name + 'Page();
                page.OnCancel += OnCancel;
                page.OnSave += OnSave;

	            await page.LoadModelAsync(model.id);

                PopupContainer1.PopupPanel = page;
                PopupContainer1.PopupPanelVisibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                PopupContainer1.PopupPanelVisibility = Visibility.Collapsed;
                await MessageBoxDialog.ShowAsync(ex.Message);
	        }
	    }

	    private async void StripPanel1_RemoveButtonClick(object sender, RoutedEventArgs e)
	    {
            var model = DataGrid1.SelectedItem as ' + @upper_case_name + 'Model;
            if (model == null) return;

            if (await ' + @upper_case_name + 'Page.RemoveUIAsync(model)) 
				RequestRefresh();
	    }

        private void DataGrid1_RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            RequestRefresh();
        }' 
		union all select
'
	    //*******************************************************************
	    //  Editing item
	    //*******************************************************************
	    private async void OnSave(object sender, RoutedEventArgs e)
	    {
	        try
	        {
                var page = (PopupContainer1.PopupPanel as ' + @upper_case_name + 'Page);
                if (page == null) return;

	            await page.SaveAsync();

                PopupContainer1.PopupPanel = null;
                PopupContainer1.PopupPanelVisibility= Visibility.Collapsed;
	        }
	        catch (Exception ex)
	        {
	            await MessageBoxDialog.ShowAsync(ex.Message);
	            return;
	        }

	        RequestRefresh();
	    }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            PopupContainer1.PopupPanel = null;
            PopupContainer1.PopupPanelVisibility= Visibility.Collapsed;
        }

        //***********************************************************
        // sorting
        //**********************************************************
        SortedButton _lastSortedButton;
        private void SortedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_lastSortedButton != sender && _lastSortedButton != null)
                _lastSortedButton.State = SortedButtonState.Default;

            _lastSortedButton = sender as SortedButton;

                RequestRefresh();
        }
        private void DataGridNavigator1_OnDataRequest(object sender, RoutedEventArgs e)
        {
            RequestRefresh();
        }
	}
}'


	declare c1 cursor local forward_only for
		select line from @lines order by id

	open c1
	declare
		@line nvarchar(max)
	fetch next from c1
		into @line
	while @@fetch_status = 0
	begin
		print @line
		fetch next from c1
			into @line
	end
	close c1
	deallocate c1

GO
	