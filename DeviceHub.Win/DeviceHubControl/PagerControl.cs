using DeviceHub.Abstractions.Dto;
using System.ComponentModel;

namespace DeviceHub.Win.DeviceHubControl
{
    /// <summary>
    /// 后台分页控件，配合 DataGridView 使用。
    /// 数据由外部按页加载，控件负责分页 UI 并在用户操作时触发 <see cref="PageChanged"/>。
    /// </summary>
    public partial class PagerControl : UserControl
    {
        private static readonly int[] DefaultPageSizeOptions = [10, 15, 20, 50, 100];

        private long _totalCount;
        private int _pageIndex = 1;
        private int _pageSize = 15;
        private bool _suppressPageChanged;

        public PagerControl()
        {
            InitializeComponent();
            InitPageSizeOptions(DefaultPageSizeOptions);
            WireEvents();
            RefreshUi();
        }

        /// <summary>总记录数</summary>
        public long TotalCount => _totalCount;

        /// <summary>当前页码（从 1 开始）</summary>
        public int PageIndex => _pageIndex;

        /// <summary>每页条数</summary>
        [DefaultValue(15)]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value;
        }

        /// <summary>总页数</summary>
        public int TotalPages =>
            _totalCount == 0 || _pageSize <= 0
                ? 0
                : (int)Math.Ceiling(_totalCount * 1.0 / _pageSize);

        /// <summary>页码或每页条数变化时触发，由外部加载对应页数据。</summary>
        public event EventHandler<PagerChangedEventArgs>? PageChanged;

        /// <summary>设置可选的每页条数。</summary>
        public void InitPageSizeOptions(IEnumerable<int> options)
        {
            _suppressPageChanged = true;
            cboPageSize.Items.Clear();
            foreach (int size in options)
            {
                cboPageSize.Items.Add(size);
            }

            if (cboPageSize.Items.Count == 0)
            {
                foreach (int size in DefaultPageSizeOptions)
                {
                    cboPageSize.Items.Add(size);
                }
            }

            if (!cboPageSize.Items.Contains(_pageSize))
            {
                _pageSize = (int)cboPageSize.Items[0]!;
            }

            cboPageSize.SelectedItem = _pageSize;
            _suppressPageChanged = false;
        }

        /// <summary>更新分页状态（不触发 <see cref="PageChanged"/>）。</summary>
        public void SetPageInfo(int pageIndex, int pageSize, long totalCount)
        {
            _suppressPageChanged = true;
            _pageIndex = Math.Max(1, pageIndex);
            _pageSize = pageSize > 0 ? pageSize : _pageSize;
            _totalCount = Math.Max(0, totalCount);

            if (TotalPages > 0 && _pageIndex > TotalPages)
            {
                _pageIndex = TotalPages;
            }

            if (cboPageSize.Items.Contains(_pageSize))
            {
                cboPageSize.SelectedItem = _pageSize;
            }

            _suppressPageChanged = false;
            RefreshUi();
        }

        /// <summary>从 <see cref="Page{T}"/> 更新分页状态（不触发 <see cref="PageChanged"/>）。</summary>
        public void SetPageInfo<T>(Page<T> page)
        {
            SetPageInfo(page.PageIndex, page.PageSize, page.TotalCount);
        }

        private void WireEvents()
        {
            btnFirst.Click += (_, _) => GoToPage(1);
            btnPrev.Click += (_, _) => GoToPage(_pageIndex - 1);
            btnNext.Click += (_, _) => GoToPage(_pageIndex + 1);
            btnLast.Click += (_, _) => GoToPage(TotalPages);
            btnJump.Click += (_, _) => JumpToInputPage();
            txtPageJump.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    JumpToInputPage();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
            cboPageSize.SelectedIndexChanged += (_, _) =>
            {
                if (_suppressPageChanged || cboPageSize.SelectedItem is not int newSize || newSize == _pageSize)
                {
                    return;
                }

                _pageSize = newSize;
                RaisePageChanged(1);
            };
        }

        private void GoToPage(int pageIndex)
        {
            if (TotalPages == 0)
            {
                return;
            }

            int target = Math.Clamp(pageIndex, 1, TotalPages);
            if (target == _pageIndex)
            {
                return;
            }

            RaisePageChanged(target);
        }

        private void JumpToInputPage()
        {
            if (TotalPages == 0)
            {
                return;
            }

            if (!int.TryParse(txtPageJump.Text.Trim(), out int target))
            {
                txtPageJump.Text = _pageIndex.ToString();
                return;
            }

            GoToPage(target);
        }

        private void RaisePageChanged(int pageIndex)
        {
            if (_suppressPageChanged)
            {
                return;
            }

            _pageIndex = pageIndex;
            RefreshUi();
            PageChanged?.Invoke(this, new PagerChangedEventArgs(_pageIndex, _pageSize));
        }

        private void RefreshUi()
        {
            lblTotal.Text = $"共 {_totalCount} 条";
            lblPageTotal.Text = $"/{TotalPages}";
            txtPageJump.Text = TotalPages == 0 ? "0" : _pageIndex.ToString();

            bool hasPages = TotalPages > 0;
            btnFirst.Enabled = hasPages && _pageIndex > 1;
            btnPrev.Enabled = hasPages && _pageIndex > 1;
            btnNext.Enabled = hasPages && _pageIndex < TotalPages;
            btnLast.Enabled = hasPages && _pageIndex < TotalPages;
            btnJump.Enabled = hasPages;
            txtPageJump.Enabled = hasPages;

            RebuildPageButtons();
            RelayoutControls();
        }

        private void RebuildPageButtons()
        {
            flpPages.SuspendLayout();
            flpPages.Controls.Clear();

            foreach (int? page in BuildVisiblePages(_pageIndex, TotalPages))
            {
                if (page == null)
                {
                    flpPages.Controls.Add(CreateEllipsisLabel());
                    continue;
                }

                flpPages.Controls.Add(CreatePageButton(page.Value));
            }

            flpPages.ResumeLayout(true);
        }

        private static IEnumerable<int?> BuildVisiblePages(int current, int totalPages)
        {
            if (totalPages <= 0)
            {
                yield break;
            }

            if (totalPages <= 7)
            {
                for (int i = 1; i <= totalPages; i++)
                {
                    yield return i;
                }

                yield break;
            }

            yield return 1;

            int left = Math.Max(2, current - 1);
            int right = Math.Min(totalPages - 1, current + 1);

            if (left > 2)
            {
                yield return null;
            }

            for (int i = left; i <= right; i++)
            {
                yield return i;
            }

            if (right < totalPages - 1)
            {
                yield return null;
            }

            yield return totalPages;
        }

        private Control CreateEllipsisLabel()
        {
            return new Label
            {
                AutoSize = true,
                Margin = new Padding(2, 6, 2, 0),
                Text = "…",
            };
        }

        private Control CreatePageButton(int page)
        {
            if (page == _pageIndex)
            {
                return new Label
                {
                    //AutoSize = true,
                    Size = new Size(56, 25),
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    Margin = new Padding(2, 2, 2, 0),
                    Padding = new Padding(6, 2, 6, 2),
                    Text = page.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
            }

            var button = new Button
            {
                //AutoSize = true,
                Size = new Size(56, 25),
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(2, 2, 2, 0),
                Padding = new Padding(4, 0, 4, 0),
                Tag = page,
                Text = page.ToString(),
                UseVisualStyleBackColor = true,
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += (_, _) => GoToPage(page);
            return button;
        }

        private void RelayoutControls()
        {
            const int top = 3;
            const int gap = 8;
            int x = 0;

            lblTotal.Location = new Point(x, 8);
            x = lblTotal.Right + gap;

            lblPageSize.Location = new Point(x, 8);
            x = lblPageSize.Right + 4;

            cboPageSize.Location = new Point(x, top);
            x = cboPageSize.Right + gap;

            btnFirst.Location = new Point(x, top);
            x = btnFirst.Right + 4;

            btnPrev.Location = new Point(x, top);
            x = btnPrev.Right + gap;

            flpPages.Location = new Point(x, top);
            x = flpPages.Right + gap;

            btnNext.Location = new Point(x, top);
            x = btnNext.Right + 4;

            btnLast.Location = new Point(x, top);
            x = btnLast.Right + gap;

            lblPagePrefix.Location = new Point(x, 8);
            x = lblPagePrefix.Right + 4;

            txtPageJump.Location = new Point(x, top);
            x = txtPageJump.Right + 4;

            lblPageTotal.Location = new Point(x, 8);
            x = lblPageTotal.Right + gap;

            btnJump.Location = new Point(x, top);
            Width = btnJump.Right;
            Height = Math.Max(32, flpPages.Bottom + 4);
        }
    }

    public sealed class PagerChangedEventArgs(int pageIndex, int pageSize) : EventArgs
    {
        public int PageIndex { get; } = pageIndex;

        public int PageSize { get; } = pageSize;
    }
}
