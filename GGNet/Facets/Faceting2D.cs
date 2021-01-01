using System;
using System.Collections.Generic;

namespace GGNet.Facets
{
    public class Faceting2D<T, TRow, TColumn> : Faceting<T>
    {
        private readonly SortedBuffer<TRow> rows = new SortedBuffer<TRow>(4, 1);
        private readonly SortedBuffer<TColumn> columns = new SortedBuffer<TColumn>(4, 1);

        private readonly IEnumerable<TRow> predefiniedRows;
        private readonly IEnumerable<TColumn> predefiniedColumns;

        private readonly Func<T, TRow> row;
        private readonly Func<T, TColumn> column;

        public Faceting2D(Func<T, TRow> row, Func<T, TColumn> column, bool freeX, bool freeY,
            IEnumerable<TRow> predefiniedRows = null, IEnumerable<TColumn> predefiniedColumns = null)
            : base(freeX, freeY)
        {
            this.row = row;
            this.column = column;

            this.predefiniedRows = predefiniedRows;
            if (predefiniedRows != null)
                rows.Add(predefiniedRows);

            this.predefiniedColumns = predefiniedColumns;
            if (predefiniedColumns != null)
                columns.Add(predefiniedColumns);
        }

        public override bool Strip => true;

        public override void Train(T item)
        {
            rows.Add(row(item));
            columns.Add(column(item));
        }

        public override void Set()
        {
            NRows = rows.Count;
            NColumns = columns.Count;

            N = NRows * NColumns;
        }

        public override (int row, int column) Map(T item)
        {
            int r = rows.IndexOf(row(item));
            int c = columns.IndexOf(column(item));

            return (r, c);
        }

        public override void Clear()
        {
            rows.Clear();
            if (predefiniedRows != null)
                rows.Add(predefiniedRows);

            columns.Clear();
            if (predefiniedColumns != null)
                columns.Add(predefiniedColumns);
        }

        public override Facet<T>[] Facets()
        {
            var facets = new Facet<T>[N];

            var i = 0;

            for (var r = 0; r < NRows; r++)
            {
                for (var c = 0; c < NColumns; c++)
                {
                    var xStrip = r == 0
                        ? columns[c].ToString()
                        : string.Empty;

                    var yStrip = c == (NColumns - 1)
                        ? rows[r].ToString()
                        : string.Empty;

                    facets[i++] = new Facet<T>(this, (r, c), xStrip, yStrip);
                }
            }

            return facets;
        }
    }
}
