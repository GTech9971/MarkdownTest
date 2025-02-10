using System.Threading.Tasks;

namespace MarkdownTest.Cli.Test;

public class ClientTest
{
    [Fact(DisplayName = "1ケースの解析")]
    public async Task single_test_case_parse()
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

        await Client.Main(["-c", markdown]);
    }

    [Fact(DisplayName = "ソリューションを解析")]
    public async Task solution_parse()
    {
        using var output = new StringWriter();
        Console.SetOut(output);

        await Client.Main(["-s", "../../../../SampleProject/SampleProject.sln"]);

        string result = output.ToString().Trim();

        Assert.True(result.Any());
    }
}
