using System;

namespace GGNet.Facets
{
    public class Faceting1D<T, TKey> : Faceting<T>
    {
        private readonly SortedBuffer<TKey> buffer = new SortedBuffer<TKey>(8, 1);

        private readonly Func<T, TKey> selector;

        private readonly int? nrows;
        private readonly int? ncolumns;

        public Faceting1D(Func<T, TKey> selector, bool freeX, bool freeY, int? nrows, int? ncolumns)
            : base(freeX, freeY)
        {
            this.selector = selector;

            this.nrows = nrows;
            this.ncolumns = ncolumns;
        }

        public override bool Strip => false;

        public override void Train(T item) => buffer.Add(selector(item));

        public override void Set()
        {
            N = buffer.Count;

            var dim = FacetingUtils.DimWrap(N, nrows, ncolumns);

            NRows = dim.nrows;
            NColumns = dim.ncolumns;
        }

        public override (int row, int column) Map(T item)
        {
            int n = buffer.IndexOf(selector(item));

            return ((int)((double)n / NColumns), n % NColumns);
        }

        public override void Clear() => buffer.Clear();

        public override Facet<T>[] Facets()
        {
            var n = buffer.Count;
            var facets = new Facet<T>[n];

            var r = 0;
            var c = 0;

            for (var i = 0; i < n; i++)
            {
                var xStrip = buffer[i].ToString();

                facets[i] = new Facet<T>(this, (r, c), xStrip);

                if (r == (NRows - 1))
                {
                    c++;
                }
                else
                {
                    if (++c == NColumns)
                    {
                        r++;
                        c = 0;
                    }
                }
            }

            return facets;
        }
    }
}
