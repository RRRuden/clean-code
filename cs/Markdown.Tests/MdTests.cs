using System;
using System.Security.Cryptography;
using Markdown.Interfaces;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Tests
    {
        public Md md;
        [SetUp]
        public void Setup()
        {
            md = new Md(new HtmlConverter());
        }
        [TestCase("_text text text_", ExpectedResult = "<em>text text text</em>", TestName = "{m}_Return_TextInItalicTag_OnSingleUnderscores")]
        [TestCase("__text text text__", ExpectedResult = "<strong>text text text</strong>", TestName = "{m}_Return_Text_In_StrongTag_On_DoubleUnderscores")]
        [TestCase("#a\n", ExpectedResult = "<h1>a</h1>\n", TestName = "{m}Return_Text_in_h1_Tag_On_Line_Starts_With_Sharp")]
        [TestCase(@"\_��� ���\_", ExpectedResult = "_��� ���_", TestName = "{m}_Return_Text_With_Shielding_Tag")]
        [TestCase(@"���\���� �������������\ \������ ��������.\", ExpectedResult = @"���\���� �������������\ \������ ��������.\", TestName = "Escaping_Char_Not_Disappear_From_Result_Unless_It_Escapes")]
        [TestCase(@"\\_��� ��� ����� �������� �����\\_", ExpectedResult = "<em>��� ��� ����� �������� �����</em>", TestName = "Escape_Char_Can_Escaped")]
        [TestCase("_12_3", ExpectedResult = "_12_3" , TestName = "Underscores_In_Text_With_Numbers_Not_Tagged")]
        [TestCase("____", ExpectedResult = "____", TestName = "TagSymbols_On_EmptyText_Still_Symbols")]
        [TestCase("� _���_���", ExpectedResult = "� <em>���</em>���", TestName = "{m}_Tagged_Part_Word_At_Start")]
        [TestCase("���_���_��", ExpectedResult = "���<em>���</em>��", TestName = "{m}_Tagged_Part_Word_At_Middle")]
        [TestCase("���_��_", ExpectedResult = "���<em>��</em>", TestName = "{m}_Tagged_Part_Word_At_End")]
        [TestCase("��_���� ��_����", ExpectedResult = "��_���� ��_����", TestName = "Tagged_In_Different_Words_Not_Work.")]
        [TestCase("__����������� _�������__ � ���������_", ExpectedResult = "__����������� _�������__ � ���������_", TestName = "Intersections_DoubleUnderscores_And_SingleUnderscores_NotTagged")]
        [TestCase("__��������_ �������", ExpectedResult = "__��������_ �������", TestName = "Unpaired_Characters_Not_Considered_Tags")]
        [TestCase("#��������� __� _�������_ ���������__", ExpectedResult = "<h1>��������� <strong>� <em>�������</em> ���������</strong></h1>", TestName = "Header_Can_Contain_Other_Markup_Elements")]
        [TestCase("_aaa__b__ccc_", ExpectedResult = "<em>aaa</em><em>b</em><em>ccc</em>", TestName = "{m}_DoubleUnderscoreInUnderscore")]
        [TestCase("__a_b___", ExpectedResult = "<strong>a<em>b</em></strong>", TestName = "{m}_UnderscoreInDoubleUnderscore")]
        [TestCase("#__a_b___\n", ExpectedResult = "<h1><strong>a<em>b</em></strong></h1>\n", TestName = "{m}_TagsInHeader")]
        public string Render_Should(string line)
        {
            var result = md.Render(line);
            TestContext.WriteLine(result);
            return result;
        }
        [Test]
        public void Render_Should_Fail_OnNull()
        {
            Action action = () => md.Render(null);
            Assert.Throws<ArgumentNullException>(() => action());
        }
    }
}