namespace sltlang.Domain.Models
{
    public class SyntaxPage : SyntaxNode
    {
        public class Member
        {
            public string Name { get; set; } = default!;
            public SyntaxNode Type { get; set; } = default!;
            public bool IsStatic { get; set; } = default!;
            public bool IsReadonly { get; set; } = default!;
            public string Value { get; set; } = default!;
        }

        public string CultureKey { get; set; } = default!;
        public Member[] Members { get; set; } = [];
        public IEnumerable<SyntaxNode> Hierarchy { get; set; } = [];
        public IEnumerable<SyntaxNode> Interfaces { get; set; } = [];
        public IEnumerable<SyntaxPage> Inheritors { get; set; } = [];
        public IEnumerable<CodeSample> CodeSamples { get; set; } = [];
    }
}
