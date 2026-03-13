# main Branch Protection Kurulumu

main branch'i korumak için GitHub'da şu adımları izle:

1. Repo sayfasında **Settings** → **Branches**
2. **Add branch protection rule** veya **Add rule**
3. Branch name pattern: `main`
4. İşaretle:
   - ✅ **Do not allow bypassing the above settings** (admin dahil)
   - ✅ **Do not allow force pushes**
   - ✅ **Do not allow deletions**
5. **Create** veya **Save changes**

İstersen ek olarak:
- **Require a pull request before merging** (PR zorunlu)
- **Require status checks to pass** (CI geçmeli)
