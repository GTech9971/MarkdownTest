using MarkdownTest.Core;

namespace MarkdownTest.Cli.Test;

public class ParserTest
{
    [Fact(DisplayName = "1テストケースのみ")]
    public void single_min_test_case()
    {
        string markdown =
            """
            ## テスト名
            ユーザー認証成功テスト

            ## テスト概要
            有効なユーザー名とパスワードを使用した場合、認証が成功することを確認する。

            ## 入力値
            - ユーザー名: test_user
            - パスワード: password123

            ## 期待値
            - 認証APIがステータスコード200を返却する。
            - レスポンスボディにaccessTokenが含まれる。

            ## 前提条件
            - データベースにユーザー情報が登録されていること。
            - 認証APIが稼働していること。

            ## 実行手順
            - 認証APIのエンドポイント/api/auth/loginに対してPOSTリクエストを送信する。
            - 入力値をリクエストボディに含める。
            - レスポンスを確認する。
            """;

        TestCase actual = MarkdownParser.Parse(markdown);

        Assert.Equal("ユーザー認証成功テスト", actual.Name);
        Assert.Equal("有効なユーザー名とパスワードを使用した場合、認証が成功することを確認する。", actual.Summary);
        Assert.Equal(["ユーザー名: test_user", "パスワード: password123"], actual.Inputs);
        Assert.Equal(["認証APIがステータスコード200を返却する。", "レスポンスボディにaccessTokenが含まれる。"], actual.ExpectedResults);
        Assert.Equal(["データベースにユーザー情報が登録されていること。", "認証APIが稼働していること。"], actual.Preconditions);
        Assert.Equal(["認証APIのエンドポイント/api/auth/loginに対してPOSTリクエストを送信する。", "入力値をリクエストボディに含める。", "レスポンスを確認する。"], actual.Steps);
    }

    [Fact(DisplayName = "1ケースのみの最大入力")]
    public void single_max_test_case()
    {
        string markdown =
                    """
            ## テスト名
            ユーザー認証成功テスト

            ## テスト概要
            有効なユーザー名とパスワードを使用した場合、認証が成功することを確認する。

            ## 入力値
            - ユーザー名: test_user
            - パスワード: password123

            ## 期待値
            - 認証APIがステータスコード200を返却する。
            - レスポンスボディにaccessTokenが含まれる。

            ## 前提条件
            - データベースにユーザー情報が登録されていること。
            - 認証APIが稼働していること。

            ## 実行手順
            - 認証APIのエンドポイント/api/auth/loginに対してPOSTリクエストを送信する。
            - 入力値をリクエストボディに含める。
            - レスポンスを確認する。

            ## 実行環境
            - OS: Windows 11
            - APIホスト: https://api.example.com
            - ツール: Postman v10.0

            ## コードリンク
            - 実装: MarkdownTest.Cli.Parser
            - テスト: MarkdownTest.Cli.Test.Parser

            ## テストID
            AUTH-001
            """;

        TestCase actual = MarkdownParser.Parse(markdown);

        Assert.Equal("ユーザー認証成功テスト", actual.Name);
        Assert.Equal("有効なユーザー名とパスワードを使用した場合、認証が成功することを確認する。", actual.Summary);
        Assert.Equal(["ユーザー名: test_user", "パスワード: password123"], actual.Inputs);
        Assert.Equal(["認証APIがステータスコード200を返却する。", "レスポンスボディにaccessTokenが含まれる。"], actual.ExpectedResults);
        Assert.Equal(["データベースにユーザー情報が登録されていること。", "認証APIが稼働していること。"], actual.Preconditions);
        Assert.Equal(["認証APIのエンドポイント/api/auth/loginに対してPOSTリクエストを送信する。", "入力値をリクエストボディに含める。", "レスポンスを確認する。"], actual.Steps);
        Assert.Equal(["OS: Windows 11", "APIホスト: https://api.example.com", "ツール: Postman v10.0"], actual.ExecuteEnvironments);
        Assert.Equal("実装: MarkdownTest.Cli.Parser", actual.CodeLink);
        Assert.Equal("テスト: MarkdownTest.Cli.Test.Parser", actual.TestCodeLink);
        Assert.Equal("AUTH-001", actual.TestId);
    }
}
