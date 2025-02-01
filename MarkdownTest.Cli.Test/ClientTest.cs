namespace MarkdownTest.Cli.Test;

public class ClientTest
{
    [Fact(DisplayName = "1ケースの解析")]
    public void single_test_case_parse()
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

        Client.Main(["-c", markdown]);
    }
}
